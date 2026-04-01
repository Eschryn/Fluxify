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

using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Fluxify.Application.Entities.Channels.Private;
using Fluxify.Application.Model;
using Fluxify.Dto.Common;

namespace Fluxify.Application.Entities.Users;

public interface IUser : IEntity, IFormattable
{
    public bool? Bot { get; }
    public string Username { get; }
    public string? Discriminator { get; }
    public string? GlobalName { get; }
    internal MediaHash? AvatarHash { get; }
    public Color? AvatarColor { get; }
    public bool? System { get; }
    public PublicUserFlags Flags { get; }

    public Task<Dm> GetOrCreateDmAsync(CancellationToken cancellationToken = default);
    
    public Uri GetAvatarUri(
        [AllowedValues(
            16, 20, 22, 24, 28, 32, 40, 44, 48, 56, 60, 64,
            80, 96, 100, 128, 160, 240, 256, 300, 320, 480,
            512, 600, 640, 1024, 1280, 1536, 2048, 3072, 4096
        )]
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    );

    Uri? GetBannerUri(
        int size = 128,
        ImageFormat format = ImageFormat.Webp,
        ImageQuality quality = ImageQuality.High,
        bool animated = false
    );
}