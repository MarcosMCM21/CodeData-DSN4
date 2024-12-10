const express = require('express');
const mysql = require('mysql');
const bodyParser = require('body-parser');
const cors = require('cors');
const crypto = require('crypto'); // Biblioteca para hash de senhas
const path = require('path');
const fs = require('fs');

const app = express();
app.use(cors());
app.use(bodyParser.json());

// Configuração do banco de dados
const db = mysql.createConnection({
  host: 'localhost',
  user: 'root',
  password: '',
  database: 'code_data',
});

// Conexão ao banco de dados
db.connect((err) => {
  if (err) {
    console.error('Erro ao conectar ao MySQL:', err);
    throw err;
  }
  console.log('Conectado ao MySQL');
});

// Servir arquivos estáticos
app.use('/uploads', express.static(path.join(__dirname, 'uploads')));

/**
 * Endpoint para buscar todos os equipamentos
 */
app.get('/equipamentos', (req, res) => {
  const sql = `
    SELECT 
      e.*, 
      es.Nome AS NomeEstoque, 
      s.Tipo AS tipoSolicitacao, 
      s.DataInicio AS dataInicio, 
      s.DataFinal AS dataFim,
      c.Nome AS nomeCliente,
      u.FirstName, 
      u.LastName, 
      u.UserName, 
      u.Email
    FROM 
      equipamentos e
    LEFT JOIN estoques es ON e.EstoqueId = es.Id
    LEFT JOIN movimentacoesequipamentos me ON me.EquipamentoId = e.Id
    LEFT JOIN solicitacoes s ON me.SolicitacaoId = s.Id
    LEFT JOIN clientes c ON s.ClienteId = c.Id
    LEFT JOIN aspnetusers u ON s.UserId = u.Id
    ORDER BY s.Tipo, e.Id;
  `;

  db.query(sql, (err, result) => {
    if (err) {
      console.error('Erro na consulta de equipamentos:', err);
      return res.status(500).send('Erro ao buscar equipamentos.');
    }
    console.log('Equipamentos encontrados:', result.length);
    res.json(result);
  });
});

/**
 * Endpoint para buscar dados do usuário
 */
app.get('/users/:username', (req, res) => {
  const username = req.params.username;

  const sql = `
    SELECT 
      FirstName, 
      LastName, 
      DataCadastro, 
      UserName, 
      Email
    FROM aspnetusers
    WHERE UserName = ?;
  `;

  db.query(sql, [username], (err, result) => {
    if (err) {
      console.error('Erro na consulta de usuário:', err);
      return res.status(500).send('Erro ao buscar os dados do usuário.');
    }

    if (result.length === 0) {
      return res.status(404).send('Usuário não encontrado.');
    }

    console.log('Usuário encontrado:', result[0]);
    res.json(result[0]);
  });
});

/**
 * Endpoint para buscar documentos de um equipamento
 */
app.get('/documento/:equipamentoId', (req, res) => {
  const equipamentoId = req.params.equipamentoId;

  const sql = `
    SELECT d.Id, d.Nome, d.Anexo, d.Tipo
    FROM documentos d
    JOIN solicitacoes s ON s.DocumentoId = d.Id
    JOIN movimentacoesequipamentos me ON me.SolicitacaoId = s.Id
    WHERE me.EquipamentoId = ?;
  `;

  db.query(sql, [equipamentoId], (err, result) => {
    if (err) {
      console.error('Erro na consulta de documento:', err);
      return res.status(500).send('Erro ao buscar documento.');
    }

    if (result.length === 0) {
      return res.status(404).send('Nenhum documento encontrado.');
    }

    const documentos = result.map((doc) => {
      const filePath = path.join(__dirname, 'uploads', doc.Nome);
      return {
        id: doc.Id,
        nome: doc.Nome,
        base64: doc.Anexo, // Dados em Base64
        tipo: doc.Tipo,
        url: fs.existsSync(filePath)
          ? `http://localhost:3000/uploads/${doc.Nome}`
          : null,
      };
    });

    console.log('Documentos encontrados:', documentos);
    res.json({ documentos });
  });
});

/**
 * Endpoint de login do usuário
 */
app.post('/login', (req, res) => {
  const { FirstName, PasswordHash } = req.body;

  const sql = `
    SELECT PasswordHash
    FROM aspnetusers
    WHERE FirstName = ?;
  `;

  db.query(sql, [FirstName], (err, result) => {
    if (err) {
      console.error('Erro na consulta de login:', err);
      return res.status(500).send('Erro ao autenticar usuário.');
    }

    if (result.length === 0) {
      return res.status(401).send('Credenciais inválidas.');
    }

    const senhaHash = result[0].PasswordHash;

    // Comparação segura
    const isPasswordMatch = crypto.timingSafeEqual(
      Buffer.from(senhaHash),
      Buffer.from(PasswordHash)
    );

    if (isPasswordMatch) {
      console.log('Login bem-sucedido para o usuário:', FirstName);
      res.status(200).send('Login bem-sucedido.');
    } else {
      console.warn('Senha incorreta para o usuário:', FirstName);
      res.status(401).send('Credenciais inválidas.');
    }
  });
});

// Iniciar o servidor
const PORT = 3000;
app.listen(PORT, '0.0.0.0', () => {
  console.log(`Servidor rodando em: http://localhost:${PORT}`);
});
