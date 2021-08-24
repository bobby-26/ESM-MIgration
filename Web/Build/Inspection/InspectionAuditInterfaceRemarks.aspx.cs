using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionAuditInterfaceRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuRemarks.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["MappingId"] = "";

            if (Request.QueryString["MappingId"] != null && Request.QueryString["MappingId"].ToString() != string.Empty)
            {
                ViewState["MappingId"] = Request.QueryString["MappingId"].ToString();

                Guid? Id = General.GetNullableGuid(ViewState["MappingId"].ToString());
                DataTable dt;

                dt = PhoenixInspectionAuditInterfaceDetails.InspectionMappingCheckitemEdit(Id);
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                }
            }
        }

    }

    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? Reviewscheduleid = General.GetNullableGuid(Request.QueryString["Reviewscheduleid"].ToString());
            Guid?   CheckItemId = General.GetNullableGuid(Request.QueryString["CHECKITEMID"].ToString());
            Guid? ChapterId = General.GetNullableGuid(Request.QueryString["CHAPTER"].ToString());
            string Description = General.GetNullableString(txtDescription.Text);

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidRemarks(Description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditInterfaceDetails.InspectionMappingCheckitemRemarks(Reviewscheduleid, CheckItemId, Description,ChapterId);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                              "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemarks(string Description)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Description == null)
            ucError.ErrorMessage = "Description is required.";


        return (!ucError.IsError);
    }

}
