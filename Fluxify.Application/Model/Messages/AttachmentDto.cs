using Fluxify.Application.Entities.Messages;

namespace Fluxify.Application.Model.Messages;

public class AttachmentDto
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public string Filename { get; set; } = "";
    public AttachmentFlags Flags { get; set; }
}