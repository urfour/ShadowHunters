using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventSystem
{
    /// <summary>
    /// Interface de tous les Listeners ajouté à un IEventManager. Voir <see cref="IEventManager.AddListener{T}(IListener{T})"/>
    /// </summary>
    /// <typeparam name="T">Type de l'événement écouté. Le <see cref="IListener{T}"/> recevra tous les événements de type <code>E</code> tel qu'il est possible d'écrire <code>T event = E;</code></typeparam>
    interface IListener<T> where T : Event
    {

        /// <summary>
        /// Fonction appelée dès qu'un IEvent est envoyé dans un IEventManager dans lequel à été ajouté ce IListener
        /// </summary>
        /// <param name="e">Evénement de type T qui a été envoyé</param>
        /// <param name="tags">Informations privées ajoutées lors de l'envoie. A utiliser uniquement pour des tâches qui ne sont pas liées à la mission spécifique de l'événement</param>
        /// <see cref="IEventManager.AddListener{T}(IListener{T})"/>
        /// <seealso cref="IEventManager.Emit(Event, string[])"/>
        void OnEvent(T e, string[] tags = null);
    }
}
