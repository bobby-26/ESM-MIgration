using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InspectionDeficiencySummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["DEFICIENCYID"] = "";
                ViewState["SOURCEID"] = "";
                ViewState["SOURCEID"] = Request.QueryString["SOURCEID"];
                ViewState["VESSELID"] = Request.QueryString["VESSELID"];
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDeficiency();
    }
    private void BindDeficiency()
    {
        try
        {
            DataSet ds = PhoenixInspectionDeficiency.ListDeficiencies(new Guid(ViewState["SOURCEID"].ToString())
                , int.Parse(ViewState["VESSELID"].ToString()));

            gvDeficiency.DataSource = ds;

            if (ViewState["DEFICIENCYID"].ToString() == "")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["DEFICIENCYID"] = ds.Tables[0].Rows[0]["FLDDEFICIENCYID"].ToString();
                    SetRowSelection();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)

                if (e.CommandName.ToUpper().Equals("SELECT"))
                {
                    RadLabel lblDeficiencyid = (RadLabel)e.Item.FindControl("lblDeficiencyid");
                    if (lblDeficiencyid != null)
                    {
                        ViewState["DEFICIENCYID"] = lblDeficiencyid.Text;
                    }
                    SetRowSelection();
                    gvMSCAT.Rebind();
                    gvPreventiveAction.Rebind();
                    gvCorrectiveAction.Rebind();
                }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCorrectiveAction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCorrectiveAction();
    }
    private void BindCorrectiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionCorrectiveAction.ListCorrectiveAction(
                General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                , null);
            gvCorrectiveAction.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCorrectiveAction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblInsCorrectActDTKey");
                if (lblDtkey != null)
                    ViewState["DTKEY"] = lblDtkey.Text;
                LinkButton ev = (LinkButton)e.Item.FindControl("imgEvidence");
                RadLabel lblIsAttchment = (RadLabel)e.Item.FindControl("lblInsCorrectActISAttachment");
                ev.Visible = SessionUtil.CanAccess(this.ViewState, ev.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();

                if (ev != null)
                {
                    if (lblIsAttchment.Text == "0")
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                        ev.Controls.Add(html);
                    }
                    else
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                        ev.Controls.Add(html);
                    }
                    //  ev.ImageUrl = Session["images"] + "/no-attachment.png";
                    ev.Attributes.Add("onclick", "openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text
                          + "&mod=" + PhoenixModule.QUALITY
                          + "&type=SHIPBOARDEVIDENCE"
                          + "&cmdname=SHIPBOARDEVIDENCEUPLOAD"
                          + "&U=0"
                          + "'); return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreventiveAction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindPreventiveAction();
    }
    private void BindPreventiveAction()
    {
        try
        {
            DataSet ds = PhoenixInspectionPreventiveAction.ListPreventiveAction(
                General.GetNullableGuid(ViewState["DEFICIENCYID"].ToString())
                , General.GetNullableGuid(null));

            gvPreventiveAction.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        // gvDeficiency.SelectedIndex = -1;
        // for (int i = 0; i < gvDeficiency.Rows.Count; i++)
        //{
        //    if (gvDeficiency.DataKeys[i].Value.ToString().Equals(ViewState["DEFICIENCYID"].ToString()))
        //    {
        //        gvDeficiency.SelectedIndex = i;
        //    }
        //}
    }

    protected void BindRCA()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionIncidentMSCAT.MSCATItemList(ViewState["DEFICIENCYID"] == null ? null : General.GetNullableGuid((ViewState["DEFICIENCYID"].ToString())));

        gvMSCAT.DataSource = ds;
    }

    protected void gvMSCAT_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindRCA();
    }
    protected void gvMSCAT_PreRender(object sender, EventArgs e)
    {
        // GridDecorator.MergeRows(gvMSCAT);
    }
    protected void gvPreventiveAction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton lnkWorkOrder = (LinkButton)e.Item.FindControl("lnkWorkOrderNumber");
                RadLabel lblWorkOrderID = (RadLabel)e.Item.FindControl("lblWorkOrderId");

                if (lnkWorkOrder != null)
                {
                    lnkWorkOrder.Attributes.Add("onclick", "openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + lblWorkOrderID.Text + "&viewonly=1'); return true;");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string strCurrentCAR = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblCAR")).Text;
                string strPreviousCAR = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblCAR")).Text;

                if (strCurrentCAR == strPreviousCAR)
                {
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;
                    previousRow.Cells[0].Visible = false;
                }

                string strCurrentIC = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblIC")).Text;
                string strPreviousIC = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblIC")).Text;

                if (strCurrentIC == strPreviousIC)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                }

                string strCurrentICRemarks = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblICRemarks")).Text;
                string strPreviousICRemarks = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblICRemarks")).Text;

                if (strCurrentICRemarks == strPreviousICRemarks)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                           previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string strCurrentBC = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblBC")).Text;
                string strPreviousBC = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblBC")).Text;

                if (strCurrentBC == strPreviousBC)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                           previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string strCurrentBCRemarks = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblBCRemarks")).Text;
                string strPreviousBCRemarks = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblBCRemarks")).Text;

                if (strCurrentICRemarks == strPreviousICRemarks)
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                           previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string strCurrentCAN = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblCAN")).Text;
                string strPreviousCAN = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblCAN")).Text;

                if (strCurrentCAN == strPreviousCAN)
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                           previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }
            }
        }
    }
}
