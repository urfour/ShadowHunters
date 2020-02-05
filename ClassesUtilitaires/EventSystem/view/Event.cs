using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EventSystem
{
    /// <summary>
    /// Interface implémenté par tous les événéments transmits via un IEventManager.
    /// Voir : <see cref="IEventManager.Emit(Event, string[])"/>
    /// </summary>
    [Serializable]
    public abstract class Event
    {

        public Event()
        {

        }

        /// <summary>
        /// Désérialize un événement à partir du string XML généré par la fonction <see cref="Event.Serialize"/> et du type de l'événement d'origine
        /// </summary>
        /// <typeparam name="T">Type d'origine de l'événement</typeparam>
        /// <param name="data">XML de l'instance d'origine</param>
        /// <returns>L'événement désérialisé</returns>
        public static T Deserialize<T>(string data) where T : Event
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            //StreamReader file = new StreamReader(path);
            return (T)serializer.Deserialize(new StringReader(data));
        }

        /// <summary>
        /// Sérialize l'événement eau format XML
        /// </summary>
        /// <returns>XML de l'instance</returns>
        public string Serialize()
        {
            XmlSerializer serializer = new XmlSerializer(this.GetType());
            StringBuilder b = new StringBuilder();
            serializer.Serialize(new StringWriter(b), this);
            return b.ToString();
        }
    }
}
