using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.StandardForm;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;


public partial class UserControlMakerModel : System.Web.UI.UserControl
{

    private string _make;
    private string _type;
    private string _componentname;
    private string _componentnumber;
    private int _ItemPerRequest = 50;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

        }
    }
    protected void gvEquipmentmaker_ItemsRequested(object sender, Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs e)
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
        ds = PhoenixCommonRegisters.Equipmenmakesearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableString(_make)
                                                      , General.GetNullableString(_type), General.GetNullableString(_componentname), General.GetNullableString(_componentnumber)
                                                      , null, null, pageNumber,
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
                ddl.Items.Add(new RadComboBoxItem(dr["FLDCOMPONENTNUMBER"].ToString(),dr["FLDMODELID"].ToString()));
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
    public string Make
    {
        set
        {
            _make = value;
        }

    }
    public string Type
    {
        set
        {
            _type = value;
        }

    }
    public string Componentname
    {
        set
        {
            _componentname = value;
        }

    }
    public string Componentnumber
    {
        set
        {
            _componentnumber= value;
        }

    }
    public bool Enabled
    {
        get
        {
            return gvEquipmentmaaker.Enabled;
        }
        set
        {
            gvEquipmentmaaker.Enabled = value;
        }
    }
    public Unit Width
    {
        get
        {
            return gvEquipmentmaaker.Width;
        }
        set
        {
            gvEquipmentmaaker.Width = value;

        }
    }
    public string CssClass
    {
        set
        {
            gvEquipmentmaaker.CssClass = value;
        }
    }

    public string SelectedValue
    {
        get
        {
            //return ViewState[gvEquipmentmaaker.ClientID] != null ? ViewState[gvEquipmentmaaker.ClientID].ToString() : "";
            return gvEquipmentmaaker.SelectedValue;
        }
        set
        {
            gvEquipmentmaaker.SelectedValue = value.ToString();
        }

    }
    public string Text
    {
        get
        {
            return gvEquipmentmaaker.Text;
        }
        set
        {
            gvEquipmentmaaker.Text = value;
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
    }


