using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;
public partial class UserControlMultiColumnPMSPTWA : System.Web.UI.UserControl
{
    private int _type = 1;
    
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

        ds = PhoenixDocumentManagementForm.FormSearch(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , null
                                                    , null
                                                    , null
                                                    , _type
                                                    , null
                                                    , null
                                                    , pageNumber
                                                    , ItemsPerRequest
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    , null
                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                    );

        //ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobSearch(
        //                                                   General.GetNullableInteger(_model),
        //                                                   null,
        //                                                   General.GetNullableString(_componentnumber),
        //                                                   null,
        //                                                   null,
        //                                                   null, null,
        //                                                   pageNumber,
        //                                                   ItemsPerRequest, ref iRowCount, ref iTotalPageCount,1 );
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            count = dt.Rows.Count;
            e.EndOfItems = (itemOffset + count) == iRowCount;
            foreach (DataRow dr in dt.Rows)
            {
                ddl.Items.Add(new RadComboBoxItem(dr["FLDCAPTION"].ToString(), dr["FLDFORMID"].ToString()));
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
    public int Type
    {
        set
        {
            _type = value;
        }

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
    public Unit DropDownWidth
    {
        set
        {
            gvMulticolumn.DropDownWidth = value;
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
}
