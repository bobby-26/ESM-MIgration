using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;

public partial class CrewIncidentsEvaluationSuggestion : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
            if (Request.QueryString["u"] == null)
            {
                toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
                MenuComment.MenuList = toolbarmain.Show();
            }
            ViewState["id"] = Request.QueryString["id"];
            if (!IsPostBack)
            {
                if (Request.QueryString["id"] != null)
                {                    
                    BindDataEdit(ViewState["id"].ToString());
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
         DataTable dt = PhoenixCrewIncidents.ListIncidents(new Guid(id), null);

         if (dt.Rows.Count > 0)
         {
             txtEvaluation.Content = dt.Rows[0]["FLDEVALUATION"].ToString();
             txtSuggestions.Content = dt.Rows[0]["FLDSUGGESTION"].ToString();
         }
    }


    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewIncidents.UpdateIncidentsEvaluation(new Guid(ViewState["id"].ToString()), txtEvaluation.Content, txtSuggestions.Content);
            }
            BindDataEdit(Request.QueryString["id"]);
            //ucStatus.Attributes["style"] = "top:0px:left:0px";
            //ucStatus.Text = "Remarks Updated";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
