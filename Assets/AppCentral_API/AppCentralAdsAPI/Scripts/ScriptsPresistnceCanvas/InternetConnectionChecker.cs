//using System.Net.NetworkInformation;
using NSubstitute;
using System.Collections;
using UnityEngine;

namespace AppCentralCore
{

    public class InternetConnectionChecker : MonoBehaviour
    {
        private const string TargetHost = "https://gameanalytics.com/";
        private const float Timeout = 2f;


        private static bool isWorkingInternet = false;

        //public static bool IsWorkingInternet { get => isWorkingInternet; private set => isWorkingInternet = value; }
        public static bool IsWorkingInternet { get => Application.internetReachability == NetworkReachability.NotReachable == true ? false : true; }


        private void Start()
        {
            //StartCoroutine(continiouslyCheckInternet());
        }

        IEnumerator continiouslyCheckInternet()
        {

            while (true)
            {

                yield return WaitForPingResponse(OnInternetConnectionChecked);
                yield return new WaitForSeconds(1);
            }

        }


        void OnInternetConnectionChecked(bool isConnected)
        {
            if (isConnected)
            {
                //IsWorkingInternet = true;
                ACLogger.UserDebug("Internet connection is available.");
            }
            else
            {
                //IsWorkingInternet = false;
                ACLogger.UserDebug("Internet connection is not available.");
            }
        }


        private IEnumerator WaitForPingResponse( System.Action<bool> callback)
        {

            //Ping ping = new Ping(TargetHost);
            //var ping = new Ping("8.8.8.8");



            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                callback.Invoke(false);

            }else
            {
                callback.Invoke(true);
            }
            yield return new WaitForSeconds(2);

            //while (!ping.isDone)
            //{

            //    yield return new WaitForFixedUpdate();
            //}

            //if (ping.time >= 0 && ping.time <= Timeout * 1000f)
            //{
            //    callback.Invoke(true); // Internet connection is available
            //}
            //else
            //{
            //    callback.Invoke(false); // Internet connection is not available
            //}

            //ping.Dispose();
        }
    }
}
