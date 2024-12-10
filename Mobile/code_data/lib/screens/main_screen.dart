import 'package:flutter/material.dart';
import 'equipamento_screen.dart';
import 'locacao_screen.dart';
import 'homologacao_screen.dart';
import 'perfil_screen.dart';

class MainScreen extends StatefulWidget {
  @override
  _MainScreenState createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> {
  int _currentIndex = 0;

  final List<Widget> _screens = [
    EquipamentoScreen(),
    LocacaoScreen(),
    HomologacaoScreen(),
    PerfilScreen(),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _screens[_currentIndex],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _currentIndex,
        onTap: (index) {
          setState(() {
            _currentIndex = index;
          });
        },
        items: [
                    BottomNavigationBarItem(
            icon: Icon(Icons.inventory),
            label: 'Estoque',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.assignment),
            label: 'Locação',
          ),

          BottomNavigationBarItem(
            icon: Icon(Icons.check_circle),
            label: 'Homologação',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person),
            label: 'Perfil',
          ),
        ],
        selectedItemColor: Color(0xFF0E6600),
        unselectedItemColor: Colors.grey,
      ),
    );
  }
}