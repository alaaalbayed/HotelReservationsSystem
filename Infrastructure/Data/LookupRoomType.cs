﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Infrastructure.Data;

public partial class LookupRoomType
{
    public long Id { get; set; }

    public string NameAr { get; set; }

    public string NameEn { get; set; }

    public virtual ICollection<LookupProperty> LookupProperty { get; set; } = new List<LookupProperty>();
}