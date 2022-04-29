using AutoMapper;
using Grpc.Core;
using GrpcService.Users;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService
{
    public class GrpcUsersService : Users.GrpcUsers.GrpcUsersBase
    {
        private ICrudService<Models.User> _service;
        private IMapper _mapper;

        public GrpcUsersService(ICrudService<Models.User> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        public override async Task<User> Create(User request, ServerCallContext context)
        {
            var result = await _service.CreateAsync(_mapper.Map<Models.User>(request));

            return _mapper.Map<User>(result);
        }

        public override async Task<Users.Users> Read(Users.Void request, ServerCallContext context)
        {
            var result = await _service.GetAsync();
            var users = new Users.Users();
            users.Collection.AddRange(result.Select(x => _mapper.Map<User>(x)).ToList());
            return users;
        }

        public override async Task<User> ReadById(Id request, ServerCallContext context)
        {
            var result = await _service.GetAsync(request.Id_);
            return _mapper.Map<User>(result);
        }

        public override async Task<Users.Void> Delete(Id request, ServerCallContext context)
        {
            await _service.DeleteAsync(request.Id_);
            return new Users.Void();
        }

        public override async Task<Users.Void> Update(User request, ServerCallContext context)
        {
            await _service.UpdateAsync(request.Id, _mapper.Map<Models.User>(request));
            return new Users.Void();
        }
    }
}
