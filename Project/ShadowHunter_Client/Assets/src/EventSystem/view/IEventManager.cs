using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem
{
    public delegate void Log(string msg);

    /// <summary>
    /// Interface d'un controlleur d'événements.
    /// </summary>
    interface IEventManager
    {
        /// <summary>
        /// Envoie un signal à tous les IListener ajouté à cette instance d'IEventManager
        /// </summary>
        /// <param name="e">IEvent à envoyé</param>
        /// <param name="tags">Informations supplémentaires privées déstinées à être traité par l'envoyeur. par convention, les valeurs commencent par le namespace suivie de la classe. Exemple : "Noyau.EventSystem.view.IEventManager:true"</param>
        void Emit(Event e, params string[] tags);

        /// <summary>
        /// Ajout un Listener dans la liste des listeners.
        /// </summary>
        /// <typeparam name="T">Type d'événements écoutés par le Listener ajouté</typeparam>
        /// <param name="listener">Listener à ajouter</param>
        void AddListener<T>(IListener<T> listener, bool MainThreaded = true) where T : Event;

        /// <summary>
        /// Enlève un Listener du controlleur.
        /// </summary>
        /// <typeparam name="T">Type du Listener à enlever</typeparam>
        /// <param name="listener">Référence du Listener à enlever</param>
        void RemoveListener<T>(IListener<T> listener) where T : Event;


        void ExecMainThreaded();

        /// <summary>
        /// Fonction d'affichage des print
        /// </summary>
        Log Log { get; set; }

        /// <summary>
        /// Fonction d'affichage des warning
        /// </summary>
        Log LogWarning { get; set; }

        /// <summary>
        /// Fonction d'affichage des erreurs. Si elle est définit elle catch les exceptions lié directement à la transmission d'un événement vers un listener.
        /// Si elle n'est pas définit, une exception sera envoyé avec le message d'erreur
        /// </summary>
        Log LogError { get; set; }
    }
}
