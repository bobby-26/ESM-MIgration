using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using Telerik.Web.UI;

public partial class CrewTravelRequestMoreInfo : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (string.IsNullOrEmpty(Request.QueryString["s"]))
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE");
                MenuComment.AccessRights = this.ViewState;
                MenuComment.MenuList = toolbarmain.Show();
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {
                    ViewState["id"] = Request.QueryString["id"];
                    BindDataEdit(ViewState["id"].ToString());
                }
                if (Request.QueryString["edityn"] != null)
                {
                    MenuComment.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataEdit(string id)
    {
        DataTable dt = new DataTable();
        dt = PhoenixCrewTravelRequest.EditTravelRequest(General.GetNullableGuid(id));
            if (dt.Rows.Count > 0)
            {
                txtComment.Content = dt.Rows[0]["FLDDETAILS"].ToString();
            }      
    }

    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewTravelRequest.TravelRequesDetailsUpdate (General.GetNullableGuid(ViewState["id"].ToString()), txtComment.Content);
            }
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo','');";
            Script += "parent.CloseCodeHelpWindow('MoreInfo');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
