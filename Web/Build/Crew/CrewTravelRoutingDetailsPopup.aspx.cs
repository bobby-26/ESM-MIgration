using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;

public partial class CrewTravelRoutingDetailsPopup : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (PhoenixSecurityContext.CurrentSecurityContext == null)
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            if (Request.QueryString["VIEWONLY"] == "false")
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SUBMIT",ToolBarDirection.Right);         
                MenuComment.MenuList = toolbarmain.Show();
            }

            if (!IsPostBack)
            {
                ViewState["ISSTOP"] = 0;

                if (Request.QueryString["VIEWONLY"] != null && Request.QueryString["VIEWONLY"] != "")
                {
                 

                }
                if (Request.QueryString["Requestforstop"] != null && Request.QueryString["Requestforstop"].ToString().Equals("1"))
                {
                    this.Title = "Stops Info";                    
                    ViewState["ISSTOP"] = 1;
                }
                BindField();

                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void RemoveEditorToolBarIcons()
    {
        txtComment.EnsureToolsFileLoaded();
        RemoveButton("ImageManager");
        RemoveButton("DocumentManager");
        RemoveButton("FlashManager");
        RemoveButton("MediaManager");
        RemoveButton("TemplateManager");
        RemoveButton("XhtmlValidator");
        RemoveButton("InsertSnippet");
        RemoveButton("ModuleManager");
        RemoveButton("ImageMapDialog");
        RemoveButton("AboutDialog");
        RemoveButton("InsertFormElement");

        txtComment.EnsureToolsFileLoaded();
        txtComment.Modules.Remove("RadEditorHtmlInspector");
        txtComment.Modules.Remove("RadEditorNodeInspector");
        txtComment.Modules.Remove("RadEditorDomInspector");
        txtComment.Modules.Remove("RadEditorStatistics");

    }

    protected void RemoveButton(string name)
    {
        foreach (EditorToolGroup group in txtComment.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }

    }
    
    private void BindField()
    {
        if ((Request.QueryString["ROUTEID"] != null) && (Request.QueryString["ROUTEID"] != ""))
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelQuotationLineItemRemarksSearch(new Guid( Request.QueryString["ROUTEID"]),
                 ViewState["ISSTOP"] == null ? null : General.GetNullableInteger(ViewState["ISSTOP"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];
            txtComment.Content = HttpUtility.HtmlDecode(dr["FLDADDITIONALINFO"].ToString());
        }
    }

    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                PhoenixCrewTravelQuoteLine.UpdateQuotationRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(Request.QueryString["ROUTEID"].ToString()), txtComment.Content,
                                         ViewState["ISSTOP"] == null ? null : General.GetNullableInteger(ViewState["ISSTOP"].ToString()));
                
            }
            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            Script += "fnReloadList();";
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
