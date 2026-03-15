using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace AdvancedFeatureDemos.Models
{
    public class Speaker
    {
        public Guid SpeakerId { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;
        [Required]
        [StringLength(100)]
        public string Company { get; set; } = null!;
        public string? Bio { get; set; }
        public int SessionsGiven { get; set; }
    }

    public class SpeakerConfiguration : IEntityTypeConfiguration<Speaker>
    {
        public void Configure(EntityTypeBuilder<Speaker> builder)
        {
            builder.ToTable("Speakers", b =>
            {
                b.IsTemporal();
                b.HasComment("Represents a speaker at a conference");
            });
        }
    }
}
