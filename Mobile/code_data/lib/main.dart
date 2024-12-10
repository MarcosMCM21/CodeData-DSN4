import 'package:flutter/material.dart';
import 'screens/main_screen.dart';
import 'screens/login_screen.dart';
import 'providers/equipamento_provider.dart';
import 'package:provider/provider.dart';

void main() {
  runApp(MyApp());
}

class MyApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return ChangeNotifierProvider(
      create: (context) => EquipamentoProvider(),
      child: MaterialApp(
        title: 'Gerenciador de Estoque',
        theme: ThemeData(primarySwatch: Colors.blue),
        initialRoute: '/', // Definindo a rota inicial
        routes: {
          '/': (context) => LoginScreen(), // Rota para a tela de login
          '/home': (context) => MainScreen(), // Rota para a MainScreen após o login
        },
      ),
    );
  }
}
