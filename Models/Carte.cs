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
    /// Carte du thème Cybersécurité — hérite de <see cref="CarteBase"/>.
    /// Implémente la propriété abstraite <see cref="NomIcone"/> avec le nom de l'icône.
    /// </summary>
    /// <remarks>
    /// Notion C# : Héritage.
    /// <c>Carte</c> extends <c>CarteBase</c> et concrétise la propriété abstraite NomIcone.
    /// </remarks>
    public class Carte : CarteBase
    {
        // ────────────── Champ privé spécifique à Carte ──────────────
        private readonly string _nomIcone;

        // ────────────── Propriété abstraite implémentée ──────────────

        /// <summary>
        /// Nom de l'icône cybersécurité (ex: "Virus", "Pare-feu").
        /// Implémentation de la propriété abstraite définie dans CarteBase.
        /// </summary>
        public override string NomIcone => _nomIcone;

        // ────────────── Constructeur ──────────────

        /// <summary>
        /// Initialise une carte cybersécurité avec son ID de paire et le nom de l'icône.
        /// Appelle le constructeur de la classe mère via <c>base(idPaire)</c>.
        /// </summary>
        /// <param name="idPaire">Identifiant numérique de paire.</param>
        /// <param name="nomIcone">Nom de l'icône cybersécurité associée.</param>
        public Carte(int idPaire, string nomIcone) : base(idPaire)
        {
            _nomIcone = nomIcone;
        }

        // ────────────── Méthodes publiques ──────────────

        /// <summary>
        /// Retourne une représentation textuelle de la carte pour le débogage.
        /// Surcharge de la méthode ToString() de CarteBase.
        /// </summary>
        public override string ToString() => $"Carte[ID={_idPaire}, Icône={_nomIcone}, État={_etat}]";
    }
}
