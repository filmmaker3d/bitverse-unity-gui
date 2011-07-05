

using System;
using System.Collections.Generic;


public interface IBitDragTransferable
{
	IBitDragHandler SourceDragHandler { get; set; }

    object[] Data { get; }
}
