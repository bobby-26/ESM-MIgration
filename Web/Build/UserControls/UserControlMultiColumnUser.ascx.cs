using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class UserControlMultiColumnUser : System.Web.UI.UserControl
{
    private bool _appenddatabounditems;
    private int _ItemPerRequest = 50;
    //public event EventHandler TextChangedEvent;
    private bool _designationrequired;
    private bool _emailrequired;
    public string _selectedemail;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {         
            ////RadMCUser.DataBind();
        }

    }
    protected void RadMCUser_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    {
        //RadComboBox ddl = (RadComboBox)sender;
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
        e.Text = e.Text != null ? ((e.Text != string.Empty && e.Text.Contains(" ")) ?e.Text.Substring(0, e.Text.IndexOf(" ")).Trim(): e.Text) : "";
        ds = PhoenixUser.UserSearch("", null, null, null, e.Text, null, "", null, null, null, pageNumber, ItemsPerRequest,
                   ref iRowCount,
                   ref iTotalPageCount);

        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            RadComboBoxItem Item;
            foreach (DataRow dr in dt.Rows)
            {
                Item = new RadComboBoxItem();

                Item.Text = dr["FLDFIRSTNAME"].ToString() != string.Empty ? dr["FLDFIRSTNAME"].ToString() : "";// + " "
                Item.Value = dr["FLDUSERCODE"].ToString();
                Item.Attributes["EMAIL"] = dr["FLDEMAIL"].ToString() != string.Empty ? dr["FLDEMAIL"].ToString() : "";
                RadMCUser.Items.Add(Item);

                //ddl.Items.Add(new RadComboBoxItem(dr["FLDFIRSTNAME"].ToString() + " " + dr["FLDMIDDLENAME"].ToString() + " " + dr["FLDLASTNAME"].ToString() + dr["FLDDESIGNATIONNAME"].ToString() != string.Empty ? " / " + dr["FLDDESIGNATIONNAME"].ToString() : "" + dr["FLDEMAIL"].ToString() != string.Empty ? " / " + dr["FLDEMAIL"].ToString() : "", dr["FLDUSERCODE"].ToString()));               
            }
        }
        //RadMCUser.DataSource = null;
        RadMCUser.DataSource = ds;
        RadMCUser.DataBind();
        RadMCUser.SelectedValue = SelectedValue;
        string message = string.Empty;
        if (iRowCount <= 0)
            message = "No matches";
        else
            message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
        e.Message = message;

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

    public string SelectedUser
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


    public string SelectedEmail
    {
        get { return hdnEmail.Value; }
        set { hdnEmail.Value = value; }
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
    public bool designationrequired
    {
        get
        {
            return _designationrequired;
        }
        set
        {
            _designationrequired = value;
        }
    }
    public bool emailrequired
    {
        get
        {
            return _emailrequired;
        }
        set
        {
            _emailrequired = value;
        }
    }

    public string ToolTip
    {
        set
        {
            RadToolTip1.Text= value;
        }
    }
    protected void RadMCUser_TextChanged(object sender, EventArgs e)
    {
        SelectedValue = RadMCUser.SelectedValue;
        if (RadMCUser.Text.ToLower().EndsWith(".com"))
        {
            SelectedEmail = RadMCUser.Text.Substring(RadMCUser.Text.LastIndexOf("/") + 1).Trim();
        }
    }

    protected void RadMCUser_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    {
        DataRowView dr = (DataRowView)e.Item.DataItem;
        e.Item.Text = e.Item.Text + " " + dr["FLDMIDDLENAME"] + " " + dr["FLDLASTNAME"];
        e.Item.Text = designationrequired == true ? (dr["FLDDESIGNATIONNAME"].ToString() != string.Empty ? e.Item.Text + " / " + dr["FLDDESIGNATIONNAME"].ToString() : e.Item.Text) : e.Item.Text;
        e.Item.Text = emailrequired == true ? (dr["FLDEMAIL"].ToString() != string.Empty ? e.Item.Text + " / " + dr["FLDEMAIL"].ToString() : e.Item.Text) : e.Item.Text;
        e.Item.Attributes["EMAIL"] = dr["FLDEMAIL"].ToString() != string.Empty ? dr["FLDEMAIL"].ToString() : "";
    }
}