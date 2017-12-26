using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;

namespace TraceRT
{
    class TraceRoute
    {
        public static string Traceroute(string ipAddressOrHostName)
        {

            PingReply pingReply = null;


            string traceResults = null;
            using (Ping pingSender = new Ping())
            {
                PingOptions pingOptions = new PingOptions();
                Stopwatch stopWatch = new Stopwatch();
                byte[] bytes = new byte[32];
                pingOptions.DontFragment = true;
                pingOptions.Ttl = 1;
                int maxHops = 30;
                traceResults += (string.Format("Tracing route to {0} over a maximum of {1} hops:", ipAddressOrHostName, maxHops)) + Environment.NewLine;
                Console.WriteLine(string.Format("Tracing route to {0} over a maximum of {1} hops:", ipAddressOrHostName, maxHops) + Environment.NewLine);


                for (int i = 1; i <= maxHops; i++)
                {
                    string[] pkg = new string[3];
                    for (int count = 0; count < 3; count++)
                    {
                        stopWatch.Reset();
                        stopWatch.Start();
                        pingReply = pingSender.Send(ipAddressOrHostName, 1000, new byte[32], pingOptions);
                        stopWatch.Stop();
                        pkg[count] = stopWatch.ElapsedMilliseconds.ToString();
                    }


                    if (pingReply.Status != IPStatus.TtlExpired && pingReply.Status != IPStatus.Success)
                    {
                        traceResults += i.ToString() + "\t*\t*\t*\t" + pingReply.Status.ToString() + Environment.NewLine;
                        Console.WriteLine(i.ToString() + "\t*\t*\t*\t" + pingReply.Status.ToString() + Environment.NewLine);
                    }

                    else
                    {
                        IPHostEntry @ipHost = Dns.Resolve(pingReply.Address.ToString());
                        traceResults += (string.Format("{0}\t{1} ms\t{2} ms\t{3} ms", i, pkg[0], pkg[1], pkg[2]));
                        Console.WriteLine(string.Format("{0}\t{1} ms\t{2} ms\t{3} ms", i, pkg[0], pkg[1], pkg[2]));

                        if (ipHost.HostName != pingReply.Address.ToString())
                        {
                            traceResults += (string.Format("\t{0} \t[{1}]", pingReply.Address, ipHost.HostName)) + Environment.NewLine;
                            Console.WriteLine(string.Format("\t{0} \t[{1}]", pingReply.Address, ipHost.HostName) + Environment.NewLine);
                        }
                        else
                        {
                            traceResults += (string.Format("\t{0}", pingReply.Address)) + Environment.NewLine;
                            Console.WriteLine(string.Format("\t{0}", pingReply.Address) + Environment.NewLine);
                        }
                    }


                    if (pingReply.Status == IPStatus.Success)
                    {
                        traceResults += ("Trace complete.") + DateTime.Now+ Environment.NewLine  ;
                        Console.WriteLine("Trace complete." + DateTime.Now + Environment.NewLine);
                        break;
                    }
                    pingOptions.Ttl++;
                }
            }

            return traceResults.ToString();
        }
    }
}
