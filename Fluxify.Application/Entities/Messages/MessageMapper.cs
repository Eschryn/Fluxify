using System.Drawing;
using Fluxify.Application.Entities.Channels;
using Fluxify.Application.Model.Messages;
using Fluxify.Application.Model.Messages.Embeds;
using Fluxify.Dto.Channels.Text.Messages;
using Fluxify.Dto.Channels.Text.Messages.Embeds;
using Riok.Mapperly.Abstractions;

namespace Fluxify.Application.Entities.Messages;

[Mapper]
public partial class MessageMapper(
    FluxerApplication application
)
{
    public int MapToInt(Color color) => color.ToArgb() & 0x00FFFFFF;
    public partial MessageAttachmentFlags MapToString(AttachmentFlags color);

    [MapValue(nameof(@MessageEmbedResponse.Children), null)]
    public partial MessageEmbedResponse MapToMessageEmbedResponse(Embed embed);
    public partial MessageRequest Map(MessageDto message);
    public async Task<Message> MapAsync(MessageResponse message) 
        => Map(message, (TextChannel)await application.Channels.GetAsync(message.ChannelId));
    public partial Message Map(MessageResponse message, TextChannel channel);
}