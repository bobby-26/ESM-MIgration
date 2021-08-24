﻿using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class UserControlMultiColumnJobs : System.Web.UI.UserControl
{   
    private int _ItemPerRequest = 50;
    public event EventHandler TextChangedEvent;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void gvMulticolumn_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
        }
        ds = PhoenixCommonPlannedMaintenance.ParameterJobSearch(null,null,null,null,
                        pageNumber,
                        ItemsPerRequest,
                        ref iRowCount,
                        ref iTotalPageCount,
                        e.Text);
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            RadComboBoxItem Item;
            foreach (DataRow dr in dt.Rows)
            {
                Item = new RadComboBoxItem();

                Item.Text = dr["FLDJOBTITLE"].ToString() != string.Empty ? dr["FLDJOBTITLE"].ToString() : "";
                Item.Value = dr["FLDJOBID"].ToString();
                gvMulticolumn.Items.Add(Item);
            }
        }
        ddl.DataSource = ds;
        ddl.DataBind();
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;

    }
   
    public bool Enabled
    {
        get
        {
            return gvMulticolumn.Enabled;
        }
        set
        {
            gvMulticolumn.Enabled = value;
        }
    }
    public int ItemsPerRequest
    {
        get
        {
            return _ItemPerRequest;
        }
        set
        {
            _ItemPerRequest = value;
        }
    }
    public Unit Width
    {
        get
        {
            return gvMulticolumn.Width;
        }
        set
        {
            gvMulticolumn.Width = value;
        }
    }
    public string CssClass
    {
        set
        {
            gvMulticolumn.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            return gvMulticolumn.SelectedValue;
        }
        set
        {
           
            gvMulticolumn.SelectedValue = value;
        }
    }
    public string Text
    {
        get
        {
            return gvMulticolumn.Text;
        }
        set
        {
            gvMulticolumn.Text = value;
        }
    }

    protected void OnTextChangedEvent(EventArgs e)
    {
        if (TextChangedEvent != null)
            TextChangedEvent(this, e);
    }

    protected void gvMulticolumn_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = gvMulticolumn.SelectedValue;
        OnTextChangedEvent(e);
    }
    public ExpandDirection ExpandDirection
    {
        set
        {
            gvMulticolumn.ExpandDirection = (RadComboBoxExpandDirection)value;
        }
    }

    public string AutoPostBack
    {
        set
        {
            if (value.ToUpper().Equals("TRUE"))
                gvMulticolumn.AutoPostBack = true;
        }
    }

    protected void gvMulticolumn_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;
        e.Item.Text = dr["FLDJOBCODE"] + " - " + e.Item.Text;
    }
}