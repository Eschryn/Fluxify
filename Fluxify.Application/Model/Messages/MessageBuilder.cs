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

using Fluxify.Application.Entities.Messages;
using Fluxify.Core.Types;
using Fluxify.Dto.Uploads;

namespace Fluxify.Application.Model.Messages;

public sealed class MessageBuilder(string? content = null)
{
    private ulong _uploadCounter;

    private MessageCreate Message { get; } = new()
    {
        Content = content,
    };
    private List<FileUpload>? _files;
    
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
        _files ??= [];
        _files.Add(upload);

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

    public MessageCreate Build()
    {
        return new MessageCreate()
        {
            AllowedMentions = Message.AllowedMentions,
            Attachments = Message.Attachments,
            Content = Message.Content,
            Embeds = Message.Embeds,
            FavoriteMemeId = Message.FavoriteMemeId,
            Flags = Message.Flags,
            Tts = Message.Tts,
            Files = _files?.ToArray(),
            MessageReference = Message.MessageReference,
            Nonce = Message.Nonce,
            Stickers = Message.Stickers
        };
    }

    public MessageBuilder WithFlags(MessageFlags compactAttachments)
    {
        Message.Flags = compactAttachments;
        return this;
    }

    public MessageBuilder WithAllowedMentions(
        bool repliedUser = false,
        AllowedMentionsParse[]? parse = null,
        Snowflake[]? roles = null,
        Snowflake[]? users = null
    ) {
        Message.AllowedMentions = new AllowedMentions
        {
            Parse = parse,
            RepliedUser = repliedUser,
            Roles = roles,
            Users = users
        };
        
        return this;
    }
}