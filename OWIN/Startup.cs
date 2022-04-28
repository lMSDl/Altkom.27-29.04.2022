using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OWIN
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.MapWhen(context => context.WebSockets.IsWebSocketRequest, webSocketApp =>
            {
                webSocketApp.Run(async context =>
                {
                    var websocket = await context.WebSockets.AcceptWebSocketAsync();
                    var helloMessage = Encoding.UTF8.GetBytes("Hello from WebSocket");
                    await websocket.SendAsync(new ArraySegment<byte>(helloMessage, 0, helloMessage.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);

                    _ = Task.Run(async () =>
                    {
                        do
                        {
                            var message = Encoding.UTF8.GetBytes("*");
                            await websocket.SendAsync(new ArraySegment<byte>(message, 0, message.Length), System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                            await Task.Delay(5000);
                        } while (!websocket.CloseStatus.HasValue);

                    });

                    do
                    {
                        byte[] buffer = new byte[1024];
                        try
                        {
                            var receive = await websocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                            await websocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), System.Net.WebSockets.WebSocketMessageType.Text, receive.EndOfMessage, CancellationToken.None);
                        }
                        catch
                        {

                        }

                    }while(!websocket.CloseStatus.HasValue);

                    await websocket.CloseAsync(websocket.CloseStatus.Value, websocket.CloseStatusDescription, CancellationToken.None);
            });


            });



            app.UseOwin(pipe => pipe(environment => OwinResponse));
        }


        private Task OwinResponse(IDictionary<string, object> enviromnemt)
        {
            string responseTest = "Hello from OWIN!";
            byte[] responseBytes = Encoding.UTF8.GetBytes(responseTest);

            var stream = (Stream)enviromnemt["owin.ResponseBody"];
            var headers = (IDictionary<string, string[]>)enviromnemt["owin.ResponseHeaders"];

            headers["Content-Type"] = new string[] { "text/plain" };
            headers["Content-Length"] = new string[] { responseBytes.Length.ToString() };

            return stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }
    }
}
