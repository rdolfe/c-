#  SecurIT Memory — Jeu de Cartes Cybersécurité

> **Projet WinForms C# · Salon de l'Innovation Tech · SecurIT**

Un jeu de Memory interactif mettant en scène 8 icônes de cybersécurité, développé en C# avec WinForms. Conçu pour le stand SecurIT au Salon de l'Innovation Tech.

---

##  Aperçu

| Menu Principal | Jeu (4×4) | Options |
|:-:|:-:|:-:|
| Écran d'accueil animé avec logo SecurIT | Grille de cartes Memory | Choix grille & difficulté |

---

##  Fonctionnalités

###  Fonctionnalités de base
- **Menu principal** avec 3 boutons : Jouer, Options, Quitter
- **Grille dynamique** 4×4 (8 paires) ou 6×6 (18 paires)
- **Mélange aléatoire** des cartes à chaque partie (algorithme Fisher-Yates)
- **Retournement** : 2 cartes maximum simultanément
- **Timer de délai** configurable avant retournement des non-paires
- **Blocage des clics** pendant le délai (protection contre la 3ème carte)
- **Chronomètre** en temps réel (MM:SS)
- **Compteur d'essais** mis à jour à chaque tentative
- **Détection de victoire** avec affichage du temps et des essais
- **Rejouer** sans relancer l'application

###  Interface
- Design **cyberpunk sombre** avec palette cyan/violet
- Icônes cybersécurité **générées en code** (GDI+, pas d'images externes)
- Animation de **pulse** sur le logo du menu
- **Dos de carte** avec circuit imprimé et logo SecurIT
- Effets de survol sur les cartes

### Icônes Cybersécurité
| Icône | Description |
|-------|-------------|
|  Virus | Créature avec pointes et yeux |
|  Pare-feu | Mur de briques + flammes |
|  Cadenas | Arc + corps + trou de serrure |
|  Mot de passe | Astérisques + barre de saisie |
|  Bouclier | Hexagone + coche de validation |
|  Hacker | Terminal avec lignes de code |
|  VPN | Globe terrestre + tunnel |
|  Chiffrement | Clé + flux de bits |

---

##  Architecture du Projet

```
SecurIT_Memory/
├── Models/
│   ├── Carte.cs          # Classe Carte (POO, encapsulation)
│   └── JeuMemory.cs      # Gestionnaire de jeu (logique, Fisher-Yates)
├── Forms/
│   ├── FormMenu.cs       # Menu principal animé
│   ├── FormJeu.cs        # Interface de jeu (grille, timers)
│   └── FormOptions.cs    # Options (grille, difficulté)
├── Utils/
│   └── IconeGenerateur.cs  # Génération d'icônes GDI+
└── Program.cs            # Point d'entrée
```

### Couches logiques
```
┌─────────────────────┐
│    FormMenu.cs      │  ← Menu principal (UI)
├─────────────────────┤
│    FormJeu.cs       │  ← Interface de jeu (WinForms)
│    FormOptions.cs   │
├─────────────────────┤
│    JeuMemory.cs     │  ← Logique de jeu (gestion état)
│    Carte.cs         │  ← Entité Carte (POO)
├─────────────────────┤
│  IconeGenerateur.cs │  ← Dessin vectoriel (GDI+)
└─────────────────────┘
```

---

##  Conception Orientée Objet

### Classe `Carte`
```csharp
public class Carte
{
    private int _idPaire;         // Identifiant de paire
    private Image? _image;        // Icône cybersécurité
    private EtatCarte _etat;      // Cachee | Revelee | Trouvee
    private string _nomIcone;     // Nom de l'icône

    // Propriétés avec get/set (encapsulation)
    public int IdPaire { get; private set; }
    public EtatCarte Etat { get; set; }
    // ...
}
```

### Énumération `EtatCarte`
```csharp
public enum EtatCarte
{
    Cachee,    // Face verso visible
    Revelee,   // Temporairement visible
    Trouvee    // Paire identifiée, reste visible
}
```

---

## ⚙️ Logique des Timers

```
Clic carte 1 → Révélation visuelle
Clic carte 2 → Révélation + Vérification paire
   ├── Paire ✓ → État = Trouvée, NbPaires++, vérifier victoire
   └── Pas paire → ClicsBloques = true
                    TimerDelai.Start() [1-2s selon difficulté]
                         ↓
                    TimerDelai_Tick() → Retourner cartes
                                        ClicsBloques = false
```

---

## 🚀 Lancement

### Prérequis
- Visual Studio 2022+ **ou** .NET 10 SDK
- Windows (WinForms)

### Via Visual Studio
1. Ouvrir `SecurIT_Memory.sln` (ou `.csproj`)
2. `F5` pour lancer en mode debug

### Via terminal
```powershell
cd "c:\Users\doudo\Documents\c#\SecurIT_Memory"
dotnet run
```

---

## 🕹️ Comment jouer

1. **Lancer** l'application → Menu Principal
2. **(Optionnel)** Cliquer sur **Options** pour choisir la grille (4×4 ou 6×6) et la difficulté
3. Cliquer sur **Jouer** pour démarrer une partie
4. **Cliquer sur deux cartes** pour les retourner
   - Si elles correspondent → elles restent visibles (paire trouvée )
   - Sinon → elles se retournent après le délai ⏱
5. Trouver toutes les paires pour **gagner** !

---

##  Équipe

| Membre | Rôle |
|--------|------|
| Binôme A | Logique de jeu (Carte, JeuMemory, Timers) |
| Binôme B | Interface WinForms (Menu, Grille, Options) |

---

## 📋 Checklist Technique

- [x] Jeu se lance sans erreur depuis Visual Studio
- [x] Menu avec Jouer / Options / Quitter fonctionnel
- [x] Classe `Carte` avec encapsulation (propriétés get/set)
- [x] Grille générée dynamiquement, cartes mélangées aléatoirement
- [x] Timer de délai (retournement des non-paires)
- [x] Blocage des clics pendant le Timer
- [x] Chronomètre et compteur d'essais affichés
- [x] Victoire détectée et affichée
- [x] Code commenté (XML doc + commentaires inline)

---

##  Technologies

- **C#** .NET 10
- **WinForms** (System.Windows.Forms)
- **GDI+** (System.Drawing) pour la génération d'icônes
- **Algorithme Fisher-Yates** pour le mélange aléatoire

---

*SecurIT © 2025 — Salon de l'Innovation Tech*
