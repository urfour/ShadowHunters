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
    /// Class implémenté par tous les événéments transmits via un IEventManager.
    /// Toute classe héritant de celle-ci doit posséder un constructeur sans parametre et des attributs public en get et en set pour que la sérialization fonctionne
    /// Voir : <see cref="IEventManager.Emit(Event, string[])"/>
    /// </summary>
    [Serializable]
    public abstract class Event
    {

        public string StackTrace { get; set; }
        public bool AlreadyEmitted { get; set; } = false;

        public Event()
        {
            StackTrace = Environment.StackTrace;
        }

        /// <summary>
        /// Désérialize un événement à partir du string XML généré par la fonction <see cref="Event.Serialize"/>
        /// </summary>
        /// <param name="data">XML de l'instance d'origine</param>
        /// <returns>L'événement désérialisé</returns>
        public static Event Deserialize(string data, bool catchInvalidType = false)
        {
            int type_length = data.IndexOf(';');
            string type_name = data.Substring(0, type_length);
            data = data.Substring(type_length + 1);
            Type t = Type.GetType(type_name);
            if (t == null)
            {
                if (catchInvalidType) return null;
                if (EventView.Manager.LogError != null) EventView.Manager.LogError("Event.Deserialize(" + type_name + ") : unkown type");
                else throw new ArgumentException("Event.Deserialize(" + type_name + ") : unkown type");
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(t);
                //StreamReader file = new StreamReader(path);
                return (Event)serializer.Deserialize(new StringReader(data));
            }
            return null;
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

            return this.GetType().FullName + ";" + b.ToString();
        }
    }
}
