﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Infrastructure.Data;

public partial class RoomTypes
{
    public long Id { get; set; }

    public double Breakfast { get; set; }

    public double Dinner { get; set; }

    public double Lunch { get; set; }

    public double ExtraBed { get; set; }

    public long TypeId { get; set; }

    public virtual LookUpProperty Type { get; set; }
}