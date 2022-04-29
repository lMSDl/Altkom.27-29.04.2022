using Grpc.Core;
using GrpcService.Stream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
{
    public class GrpcStreamService : Stream.GrpcStream.GrpcStreamBase
    {

        public override async Task<Response> ToServer(IAsyncStreamReader<Request> requestStream, ServerCallContext context)
        {
            var resposne = new Response();

            await foreach (var request in requestStream.ReadAllAsync())
            {
                resposne.Text += request.Text;
            }

            return resposne;
        }

        public override async Task FromServer(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            var text = "Ala ma kota i dwa psy";
            foreach (var item in text)
            {
                await responseStream.WriteAsync(new Response { Text = item.ToString() });
            }

        }

        public override async Task FromAndToServer(IAsyncStreamReader<Request> requestStream, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            await foreach (var request in requestStream.ReadAllAsync())
            {
                await responseStream.WriteAsync(new Response { Text = request.Text });
            }
        }
    }
}
