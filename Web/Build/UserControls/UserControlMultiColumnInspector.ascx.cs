using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

[ValidationPropertyAttribute("SelectedValue")]
public partial class UserControlMultiColumnInspector : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
        }
    }
    public bool Enabled
    {
        get
        {
            return RadMCUser.Enabled;
        }
        set
        {
            RadMCUser.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return RadMCUser.Width;
        }
        set
        {
            RadMCUser.Width = value;

        }
    }
    public string CssClass
    {
        set
        {
            RadMCUser.CssClass = value;
        }
    }
    public string SelectedValue
    {
        get
        {
            return ViewState[RadMCUser.ClientID] != null ? ViewState[RadMCUser.ClientID].ToString() : "";
        }
        set
        {
            ViewState[RadMCUser.ClientID] = value;
        }

    }
    public string Text
    {
        get
        {
            return RadMCUser.Text;
        }
        set
        {
            RadMCUser.Text = value;
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
    public bool AppendDataBoundItems
    {
        set
        {
            _appenddatabounditems = value;
        }
    }

    protected void RadMCUser_ItemDataBound(object sender, Telerik.Web.UI.RadComboBoxItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;
        e.Item.Text =  e.Item.Text + (dr["FLDDESIGNATIONNAME"].ToString() != string.Empty ? " - " + dr["FLDDESIGNATIONNAME"].ToString():"");
    }

    protected void RadMCUser_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = RadMCUser.SelectedValue;
    }

    protected void RadMCUser_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
    {
        RadComboBox ddl = (RadComboBox)sender;
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int pageNumber = 1;
        int itemOffset = e.NumberOfItems;
        int count = 0;
        DataSet ds = new DataSet();
        if (e.NumberOfItems > 0)
        {
            pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
        }

        ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(e.Text,
                                            null, null,
                                            pageNumber,
                                            ItemsPerRequest,
                                            ref iRowCount,
                                            ref iTotalPageCount);

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new RadComboBoxItem(dr["FLDUSERNAME"].ToString() + dr["FLDDESIGNATIONNAME"].ToString() != string.Empty ? " - " + dr["FLDDESIGNATIONNAME"].ToString() : "", dr["FLDUSERCODE"].ToString()));
            }
        }
        ddl.DataSource = ds;
        ddl.DataBind();
        ddl.SelectedValue = SelectedValue;
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;
    }
}
