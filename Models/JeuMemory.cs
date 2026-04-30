using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurIT_Memory.Models
{
    /// <summary>
    /// Gestionnaire central du jeu Memory SecurIT.
    /// Orchestre le mélange, la sélection et la vérification des cartes.
    /// </summary>
    public class JeuMemory
    {
        // ────────────── Champs privés ──────────────
        private List<Carte> _cartes;
        private List<Carte> _cartesRetournees;
        private int _nbEssais;
        private int _nbPairesTrouvees;
        private int _totalPaires;
        private bool _clicsBloqués;
        private Random _random;

        // ────────────── Noms des icônes cybersécurité ──────────────
        /// <summary>Thème Cybersécurité — icônes disponibles.</summary>
        public static readonly string[] IconesCybersec = new[]
        {
            "Virus",
            "Pare-feu",
            "Cadenas",
            "Mot de passe",
            "Bouclier",
            "Hacker",
            "VPN",
            "Chiffrement"
        };

        // ────────────── Propriétés publiques ──────────────

        /// <summary>Liste complète des cartes du jeu (mélangées).</summary>
        public List<Carte> Cartes => _cartes;

        /// <summary>Nombre d'essais effectués par le joueur.</summary>
        public int NbEssais => _nbEssais;

        /// <summary>Nombre de paires trouvées.</summary>
        public int NbPairesTrouvees => _nbPairesTrouvees;

        /// <summary>Indique si tous les clics sont temporairement bloqués (délai Timer).</summary>
        public bool ClicsBloques
        {
            get => _clicsBloqués;
            set => _clicsBloqués = value;
        }

        /// <summary>Indique si la partie est terminée (toutes les paires trouvées).</summary>
        public bool EstTermine => _nbPairesTrouvees == _totalPaires;

        // ────────────── Constructeur ──────────────

        /// <summary>
        /// Initialise un nouveau jeu Memory avec le nombre de paires spécifié.
        /// </summary>
        /// <param name="nbPaires">Nombre de paires de cartes (ex: 8 pour une grille 4×4).</param>
        public JeuMemory(int nbPaires)
        {
            _random = new Random();
            _cartes = new List<Carte>();
            _cartesRetournees = new List<Carte>();
            _nbEssais = 0;
            _nbPairesTrouvees = 0;
            _totalPaires = nbPaires;
            _clicsBloqués = false;

            InitialiserCartes(nbPaires);
            MelangerCartes();
        }

        // ────────────── Méthodes privées ──────────────

        /// <summary>
        /// Crée les paires de cartes avec les icônes cybersécurité.
        /// </summary>
        private void InitialiserCartes(int nbPaires)
        {
            // S'assure de ne pas dépasser le nombre d'icônes disponibles
            int nbIcones = Math.Min(nbPaires, IconesCybersec.Length);

            for (int i = 0; i < nbIcones; i++)
            {
                string nomIcone = IconesCybersec[i % IconesCybersec.Length];
                // Crée deux cartes identiques (une paire)
                _cartes.Add(new Carte(i, nomIcone));
                _cartes.Add(new Carte(i, nomIcone));
            }
        }

        /// <summary>
        /// Mélange aléatoirement la liste de cartes avec l'algorithme Fisher-Yates.
        /// </summary>
        private void MelangerCartes()
        {
            for (int i = _cartes.Count - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1);
                (_cartes[i], _cartes[j]) = (_cartes[j], _cartes[i]);
            }
        }

        // ────────────── Méthodes publiques ──────────────

        /// <summary>
        /// Tente de révéler une carte. Retourne false si l'action est impossible.
        /// </summary>
        /// <param name="carte">La carte sur laquelle le joueur a cliqué.</param>
        /// <returns>True si la carte peut être révélée, false sinon.</returns>
        public bool TenterRevelerCarte(Carte carte)
        {
            // Bloque si les clics sont gelés ou si 2 cartes sont déjà retournées
            if (_clicsBloqués) return false;
            if (!carte.EstCachee()) return false;
            if (_cartesRetournees.Count >= 2) return false;

            carte.Etat = EtatCarte.Revelee;
            _cartesRetournees.Add(carte);
            return true;
        }

        /// <summary>
        /// Vérifie si les deux cartes retournées forment une paire.
        /// Retourne true si c'est une paire, false sinon.
        /// </summary>
        public bool VerifierPaire()
        {
            if (_cartesRetournees.Count != 2) return false;

            _nbEssais++;
            Carte c1 = _cartesRetournees[0];
            Carte c2 = _cartesRetournees[1];

            if (c1.IdPaire == c2.IdPaire)
            {
                // Paire trouvée !
                c1.Etat = EtatCarte.Trouvee;
                c2.Etat = EtatCarte.Trouvee;
                _nbPairesTrouvees++;
                _cartesRetournees.Clear();
                return true;
            }
            else
            {
                // Pas une paire — le Timer appellera RetournerCartesNonPairedees()
                return false;
            }
        }

        /// <summary>
        /// Retourne face cachée les deux cartes non-appariées après le délai du Timer.
        /// </summary>
        public void RetournerCartesNonPairedees()
        {
            foreach (var carte in _cartesRetournees)
            {
                if (carte.Etat == EtatCarte.Revelee)
                    carte.Etat = EtatCarte.Cachee;
            }
            _cartesRetournees.Clear();
            _clicsBloqués = false;
        }

        /// <summary>
        /// Retourne le nombre de cartes encore retournées en attente de vérification.
        /// </summary>
        public int NbCartesRetournees => _cartesRetournees.Count;
    }
}
