using Foundation;
using UIKit;
using WindowsAzure.Messaging;


namespace NotificationHub
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        //Declaracion de variables para las notificaciones push
        private SBNotificationHub Hub { get; set; }
        public const string ConnectionString = "Azure-EndPoint"; // DefaultListenSharedAccessSignature
        public const string NotificationHubPath = "Nombre-AzureNotificationPush"; 

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // Override point for customization after application launch.
            // If not required for your application you can safely delete this method

            // Code to start the Xamarin Test Cloud Agent
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

            var settings = UIUserNotificationSettings.GetSettingsForTypes(UIUserNotificationType.Alert 
                                                                          | UIUserNotificationType.Badge 
                                                                          | UIUserNotificationType.Sound
                                                                          , new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Es invocado cuando la aplicacion está a punto de cambiar de estado activo a inactivo
            // Esto puede ocurrir por varios tipos de interrupciones temporales (tales como una llamada de telefono o SMS)
            // o cuando el usuario quita la aplicacion y comienza la transicion a estado de segundo plano.
            // Los juegos suele usar este metodo para pausar la partida.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Usa este metodo para lanzar los recursos compartidos, guardar datos de usuario, invalidar relojes y almacenar el
            // estado de la aplicacion.
            // Si la aplicacion soporta tareas en segundo plano este metodo es llamado en lugar de WillTerminate cuando el usuario
            // cierra la aplicacion.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Llamado como parte de la transicion desde la tarea de segundo plano al estado activo de la aplicacion.
            // Aqui se pueden deshacer la mayoria de los cambios realizados cuando se entró en segundo plano.
        }

        public override void OnActivated(UIApplication application)
        {
            // Reinicia cualquier tarea pausada (o no comenzada) mientras la aplicacion estuvo inactiva.
            // Si la aplicación estuvo previamente en segundo plano, opcionalmente refresca la interfaz de usuario.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Es llamado cuando la aplicacion está a punto de terminar. Guardar datos si es necesario. 
            // Ver tambien DidEnterBackgroudn.
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //Creamos un nuevo hub de notificaciones con la cadena de conexion y la ruta del hub
            Hub = new SBNotificationHub(ConnectionString, NotificationHubPath);

            //Eliminamos los registros que pudieran haber de instancias anteriores
            Hub.UnregisterAllAsync(deviceToken, (error) =>
            {
                if (error != null)
                {
                    //Error al eliminar los registros
                    return;
                }

            });

            //Registrar el dispositivo para las notificaciones
            Hub.RegisterNativeAsync(deviceToken, null, (registerError) =>
            {
                if (registerError != null)
                {
                    //No se pudo realizar el registro
                }
            });
        }
        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
       
            //Este metodo es llamado cuando se recive una notificacion remota
            //Si la aplicacion esta en primer plano y no de fondo

            if(null != userInfo && userInfo.ContainsKey(new NSString("aps")))
            {
                NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;
            }

        }
    }
}

