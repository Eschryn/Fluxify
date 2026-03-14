using Fluxify.Application.Entities.Messages;
using Fluxify.Core.Types;
using Fluxify.Dto.Uploads;

namespace Fluxify.Application.Model.Messages;

public sealed class MessageBuilder(string? content = null)
{
    private ulong _uploadCounter;
    
    public MessageDto Message { get; } = new()
    {
        Content = content,
    };
    
    public MessageBuilder WithContent(string content)
    {
        Message.Content = content;
        return this;
    }

    public MessageBuilder WithAttachment(
        Stream stream,
        string filename,
        string contentType,
        string? title = null,
        string? description = null,
        AttachmentFlags? flags = null
    ) => WithAttachment(
        new StreamFileUpload(
            stream,
            filename,
            contentType,
            new Snowflake(_uploadCounter++)),
        title,
        description,
        flags);
    
    public MessageBuilder WithAttachment(
        byte[] array,
        string filename,
        string contentType,
        string? title = null,
        string? description = null,
        AttachmentFlags? flags = null
    ) => WithAttachment(
        new ArrayFileUpload(
            array,
            filename,
            contentType,
            _uploadCounter++),
        title,
        description,
        flags);
    
    private MessageBuilder WithAttachment(
        FileUpload upload,
        string? title = null,
        string? description = null,
        AttachmentFlags? flags = null
    )
    {
        Message.Attachments ??= [];
        Message.Attachments.Add(new Attachment
        {
            Id = upload.SendId,
            Description = description,
            Title = title,
            Filename = upload.FileName,
            Flags = flags ?? AttachmentFlags.None
        });
        Message.Files ??= [];
        Message.Files.Add(upload);

        return this;
    }

    public MessageBuilder WithEmbed(Action<EmbedBuilder> builder)
    {
        Message.Embeds ??= [];
        
        var embedBuilder = new EmbedBuilder();
        builder(embedBuilder);
        Message.Embeds.Add(embedBuilder.Build());

        return this;
    }

    public MessageDto Build() => Message;

    public MessageBuilder WithFlags(MessageFlags compactAttachments)
    {
        Message.Flags = compactAttachments;
        return this;
    }

    public MessageBuilder WithAllowedMentions(AllowedMentions allowedMentions)
    {
        Message.AllowedMentions = allowedMentions;
        return this;
    }
}