﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AppCode;

public partial class Public_Init : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        InitModel.Refresh();
    }
}