using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
using SouthNests.Phoenix.Integration;

public partial class InspectionAuditInterfaceFormCheckItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void gvInspectionForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Guid CheckItemId = new Guid(Request.QueryString["CHECKITEMID"].ToString());

        // DataSet ds  = PhoenixInspectionVesselCheckItems.VesselCheckitemProcedureFormsList(CheckItemId);

        DataSet ds = PhoenixInspectionRegisterCheckItems.FormsList(CheckItemId);
        gvInspectionForm.DataSource = ds;
    }


    protected void gvInspectionForm_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvInspectionForm_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                int type = 0;
                string Formid = (((RadLabel)e.Item.FindControl("lblFormPosterId")).Text);
                LinkButton form = ((LinkButton)item.FindControl("lnkForm"));
                if (form != null)
                {

                    PhoenixIntegrationQuality.GetSelectedeTreeNodeType(
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblFormPosterId")).Text)
                        , ref type);


                    if (type == 2)
                        form.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + Formid + "');return false;");
                    else if (type == 3)
                        form.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + Formid + "');return false;");

                }


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
}