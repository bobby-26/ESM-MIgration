using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class WorkflowRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Purchase/WorkflowRequest.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Purchase/WorkflowRequest.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuWFOrderList.AccessRights = this.ViewState;
            MenuWFOrderList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvWFOrderList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);            
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWFOrderList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWFOrderList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            int? vesselid = General.GetNullableInteger(UcVessel.SelectedVessel);

            DataTable dt =  PhoenixWorkflow.ordersearch(vesselid,
                sortexpression, sortdirection, gvWFOrderList.CurrentPageIndex + 1,
                gvWFOrderList.PageSize, ref iRowCount, ref iTotalPageCount);

            gvWFOrderList.DataSource = dt;
            gvWFOrderList.VirtualItemCount = iRowCount;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

    protected void MenuWFOrderList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
                     
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                UcVessel.SelectedVessel = "";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvWFOrderList.SelectedIndexes.Clear();
        gvWFOrderList.EditIndexes.Clear();
        gvWFOrderList.DataSource = null;
        gvWFOrderList.Rebind();
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }


    protected void gvWFOrderList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                PhoenixWorkflow.WorkflowRequest(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOrderId")).Text),
                    General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblVesselId")).Text),
                    "Purchase_Requisition", "NEW");

                PhoenixWorkflow.WorkflowRequestDataUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   General.GetNullableString(((RadLabel)e.Item.FindControl("lblOrderId")).Text)
                        );

                ucStatus.Text = "Workflow Request Added.";
                Rebind();
            }
      
            if(e.CommandName.ToUpper().Equals("ACTION"))
            {
                DataTable D = new DataTable();
                D = PhoenixWorkflow.WorkflowRequestDataEdit(General.GetNullableString(((RadLabel)e.Item.FindControl("lblOrderId")).Text));
                if (D.Rows.Count > 0)
                {
                    DataRow d = D.Rows[0];
                    ViewState["REQUESTID"] = d["FLDREQUESTID"].ToString();
                    ViewState["PROCESSID"] = d["FLDPROCESSID"].ToString();            
                    string jsonstring = General.DataTableToJSONWithJSONNet(D);

                    PhoenixWorkflow.WorkflowRequestUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["REQUESTID"].ToString()), jsonstring);
                
                    PhoenixWorkflow.WorkflowRequestTransitionActionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        "Purchase_Requisition", General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

                    PhoenixWorkflow.WorkflowInboxInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        "Purchase_Requisition", "APR", General.GetNullableGuid(ViewState["REQUESTID"].ToString()));

                    ucStatus.Text = "Workflow Request Transition Action Added.";
                    Rebind();
                }

            }      
       }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWFOrderList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;  
                        
                LinkButton Request = ((LinkButton)item.FindControl("cmdRequest"));
                if (Request != null)
                {                 
                    DataTable D1 = new DataTable();
                    D1 = PhoenixWorkflow.WorkflowRequestDataEdit(General.GetNullableString(((RadLabel)e.Item.FindControl("lblOrderId")).Text));
                    if (D1.Rows.Count > 0)
                    {
                        DataRow dr1 = D1.Rows[0];                      
                       Request.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','WorkflowRequest','Purchase/WorkflowRequestList.aspx?REQUESTID=" + General.GetNullableGuid(dr1["FLDREQUESTID"].ToString()) + "&PROCESSID=" + General.GetNullableGuid(dr1["FLDPROCESSID"].ToString()) + " ');return false");
                   }
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