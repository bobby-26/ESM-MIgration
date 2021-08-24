using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class InspectionPersonalGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {        
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentCrewSelection = Request.QueryString["empid"];
           
        }
        if (!string.IsNullOrEmpty(Request.QueryString["PNIID"]))
        {
            ViewState["PNIID"] = Request.QueryString["PNIID"];
        }
        if (!string.IsNullOrEmpty(Request.QueryString["REFNO"]))
        {
            Title1.Text = "P&I Medical Case" + "(" + Request.QueryString["REFNO"] + ")";
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Medical Case", "MEDICALCASE");
        toolbarmain.AddButton("Corres.", "CORRESPONDENCE");
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbarmain.Show();
        CrewMenu.SelectedMenuIndex = 1;
        if (!Page.IsPostBack)
        {
            ViewState["email"]  = Request.QueryString["email"];
            ViewState["empid"] = Request.QueryString["empid"];
        }
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        if (ViewState["email"] != null)
        {
            toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Corres.", "SUBCORRESPONDENCE");
            toolbarsub.AddButton("Email", "EMAIL");
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbarsub.Show();
            CrewMenuGeneral.SelectedMenuIndex = 1;
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionCorrespondenceEmail.aspx?cid=" + Request.QueryString["cid"] + "&PNIID=" + Request.QueryString["PNIID"];
            return;
        }
        else if (ViewState["empid"] != null)
        {
            toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Corres.", "SUBCORRESPONDENCE");
            toolbarsub.AddButton("Email", "EMAIL");
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbarsub.Show();
            CrewMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionCorrespondence.aspx?empid=" + Request.QueryString["empid"] + "&PNIID=" + Request.QueryString["PNIID"];
            return;
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Corres.", "SUBCORRESPONDENCE");
			toolbarsub.AddButton("Email", "EMAIL");
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbarsub.Show();
            CrewMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../Inspection/InspectionCorrespondence.aspx";
        }
        else if (CommandName.ToUpper().Equals("MEDICALCASE"))
        {
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIID=" + ViewState["PNIID"], true);
            }

        }
        
    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EMAIL"))
        {
            CrewMenuGeneral.SelectedMenuIndex = 1;
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inspection/InspectionCorrespondenceEmail.aspx?PNIID="+ViewState["PNIID"].ToString();

        }
        else if (CommandName.ToUpper().Equals("SUBCORRESPONDENCE"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Inspection/InspectionCorrespondence.aspx?PNIID=" + ViewState["PNIID"].ToString();
        }
        ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();

    }

    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {

            DataTable dt = PhoenixIntegrationQuality.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));
            if (dt.Rows.Count > 0)
            {
                dtkey = new Guid(dt.Rows[0]["FLDDTKEY"].ToString());
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return dtkey;
    }
}
