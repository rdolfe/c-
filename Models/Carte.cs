using System.Drawing;

namespace SecurIT_Memory.Models
{
    /// <summary>
    /// Représente l'état possible d'une carte dans le jeu Memory.
    /// </summary>
    public enum EtatCarte
    {
        /// <summary>La carte est face cachée (dos visible).</summary>
        Cachee,
        /// <summary>La carte est temporairement retournée (face visible).</summary>
        Revelee,
        /// <summary>La paire a été trouvée, la carte reste visible.</summary>
        Trouvee
    }

    /// <summary>
    /// Classe représentant une carte du jeu Memory SecurIT.
    /// Encapsule l'identifiant de paire, l'image et l'état de la carte.
    /// </summary>
    public class Carte
    {
        // ────────────── Champs privés ──────────────
        private int _idPaire;
        private Image? _image;
        private EtatCarte _etat;
        private string _nomIcone;

        // ────────────── Propriétés publiques ──────────────

        /// <summary>
        /// Identifiant de paire : deux cartes avec le même ID forment une paire valide.
        /// </summary>
        public int IdPaire
        {
            get => _idPaire;
            private set => _idPaire = value;
        }

        /// <summary>
        /// Image affichée quand la carte est révélée (icône cybersécurité).
        /// </summary>
        public Image? Image
        {
            get => _image;
            set => _image = value;
        }

        /// <summary>
        /// État courant de la carte (Cachee, Revelee, Trouvee).
        /// </summary>
        public EtatCarte Etat
        {
            get => _etat;
            set => _etat = value;
        }

        /// <summary>
        /// Nom de l'icône cybersécurité (ex: "Virus", "Pare-feu"...).
        /// </summary>
        public string NomIcone
        {
            get => _nomIcone;
            private set => _nomIcone = value;
        }

        // ────────────── Constructeur ──────────────

        /// <summary>
        /// Initialise une nouvelle carte avec son identifiant de paire et le nom de l'icône.
        /// </summary>
        /// <param name="idPaire">Identifiant numérique de paire.</param>
        /// <param name="nomIcone">Nom de l'icône cybersécurité associée.</param>
        public Carte(int idPaire, string nomIcone)
        {
            _idPaire = idPaire;
            _nomIcone = nomIcone;
            _etat = EtatCarte.Cachee;
            _image = null;
        }

        // ────────────── Méthodes publiques ──────────────

        /// <summary>
        /// Indique si la carte est actuellement cachée (dos visible).
        /// </summary>
        public bool EstCachee() => _etat == EtatCarte.Cachee;

        /// <summary>
        /// Indique si la paire de cette carte a été trouvée.
        /// </summary>
        public bool EstTrouvee() => _etat == EtatCarte.Trouvee;

        /// <summary>
        /// Retourne une représentation textuelle de la carte pour le débogage.
        /// </summary>
        public override string ToString() => $"Carte[ID={_idPaire}, Icône={_nomIcone}, État={_etat}]";
    }
}
