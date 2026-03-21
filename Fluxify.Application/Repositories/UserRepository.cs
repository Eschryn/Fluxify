// Copyright 2026 Fluxify Contributors
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Fluxify.Application.Entities.Users;
using Fluxify.Application.State;
using Fluxify.Core.Types;
using Fluxify.Dto.Users;
using Fluxify.Rest;

namespace Fluxify.Application.Repositories;

public class UserRepository(RestClient client, Entities.Users.UserMapper mapper)
{
   public readonly PermanentCache<GlobalUser, Entities.Users.UserMapper> Cache = new(mapper);
   
   public async Task<GlobalUser> GetAsync(Snowflake id, bool bypassCache = false) 
      => await Cache.GetOrCreateAsync(id, GetUserRestAsync, bypassCache);

   internal GlobalUser Insert(UserPartialResponse response)
      => Cache.UpdateOrCreate(mapper.Map(response));

   internal GlobalUser Insert(UserPrivateReponse response)
      => Cache.UpdateOrCreate(mapper.Map(response));

   internal GlobalUser? GetCachedOrDefault(Snowflake id) => Cache.GetCachedOrDefault<GlobalUser>(id);
   
   private async Task<GlobalUser> GetUserRestAsync(Snowflake id) 
      => await client.Users[id].GetAsync() is {} user 
         ? mapper.Map(user) 
         : throw new Exception($"Couldn't get user with id {id}");
   
   internal void Reset() => Cache.Clear();
}