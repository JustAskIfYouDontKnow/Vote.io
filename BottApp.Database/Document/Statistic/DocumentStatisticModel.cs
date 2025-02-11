﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BottApp.Database.User;

namespace BottApp.Database.Document.Statistic;
 
public class DocumentStatisticModel : AbstractModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int DocumentId { get; set; }

    [ForeignKey("DocumentId")]
    public DocumentModel DocumentModel { get; set; }

    [Required]
    public int ViewCount { get; set; }

    [Required]
    public int LikeCount { get; set; }

    public bool IsModerated { get; set; } = false;


    public static DocumentStatisticModel CreateModel(int documentId, int viewCount, int likeCount)
    {
        return new DocumentStatisticModel()
        {
            DocumentId = documentId,
            ViewCount = viewCount,
            LikeCount = likeCount
        };
    }
    
    public static DocumentStatisticModel CreateEmpty(int viewCount = 0, int likeCount = 0)
    {
        return new DocumentStatisticModel()
        {
            ViewCount = viewCount,
            LikeCount = likeCount
        };
    }
    
}