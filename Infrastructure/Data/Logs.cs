﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Infrastructure.Data;

public partial class Logs
{
    public int Id { get; set; }

    public string Level { get; set; }

    public string Class { get; set; }

    public string Method { get; set; }

    public string Exception { get; set; }

    public string Message { get; set; }

    public DateTime Timestamp { get; set; }
}