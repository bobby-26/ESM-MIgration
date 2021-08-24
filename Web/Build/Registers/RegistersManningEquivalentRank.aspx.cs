using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;


public partial class Registers_RegistersManningEquivalentRank : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem r in gvRankList.Items)
        {

            Page.ClientScript.RegisterForEventValidation(gvRankList.UniqueID, "Select$" + r.RowIndex.ToString());

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["MANNINGSCALEKEY"] = "0";
                if (Request.QueryString["manningscalekey"] != null)
                {
                    ViewState["MANNINGSCALEKEY"] = Request.QueryString["manningscalekey"].ToString();
                }
                ViewState["READONLY"] = "";
                if (Request.QueryString["readonly"] != null)
                {
                    ViewState["READONLY"] = Request.QueryString["readonly"].ToString();

                }
            }
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds;
        ds = PhoenixRegistersVesselManningScale.ListEquivalentRank(new Guid(ViewState["MANNINGSCALEKEY"].ToString()), General.GetNullableByte(ViewState["READONLY"].ToString() == "1" ? "1" : ""));

        gvRankList.DataSource = ds.Tables[0];

    }


    //protected void gvRankList_RowCreated(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
    //        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
    //    {
    //        e.Row.TabIndex = -1;
    //        e.Row.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvRankList, "Select$" + e.Row.RowIndex.ToString(), false);
    //    }
    //}




    //protected void gvRankList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    //{

    //    try
    //    {
    //        gvRankList.SelectedIndex = e.NewSelectedIndex;

    //        ViewState["EQUIVALENTRANK"] = ((Label)gvRankList.Rows[e.NewSelectedIndex].FindControl("lblRankId")).Text;

    //        CheckBox chkSelect = ((CheckBox)gvRankList.Rows[e.NewSelectedIndex].FindControl("chkSelect"));
    //        if (chkSelect.Checked == true)
    //            UpdateEquivalentRank();
    //        else
    //            DeleteEquivalentRank();

    //        if (gvRankList.EditIndex > -1)
    //            gvRankList.UpdateRow(gvRankList.EditIndex, false);

    //        gvRankList.EditIndex = -1;
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        BindData();
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}

    private void DeleteEquivalentRank()
    {
        PhoenixRegistersVesselManningScale.DeleteEquivalentRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , new Guid(ViewState["MANNINGSCALEKEY"].ToString())
                                                                 , int.Parse(ViewState["EQUIVALENTRANK"].ToString())
                                                                 );
    }
    private void UpdateEquivalentRank()
    {
        PhoenixRegistersVesselManningScale.UpdateEquivalentRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , new Guid(ViewState["MANNINGSCALEKEY"].ToString())
                                                                , int.Parse(ViewState["EQUIVALENTRANK"].ToString())
                                                                );
    }

    protected void gvRankList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRankList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {

            e.Item.Cells[0].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvRankList, "Select$" + e.Item.RowIndex.ToString(), false);
            CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");

            if (ViewState["READONLY"].ToString() == "1")
            {
                gvRankList.Columns[0].Visible = false;
            }
            if (General.GetNullableInteger(drv["FLDEQUIVALENTRANK"].ToString()) != null)
            {
                chkSelect.Checked = true;
            }


        }
    }

  
    public void chkSelect_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cb = (CheckBox)sender;
            GridDataItem gvRow = (GridDataItem)cb.Parent.Parent;

            CheckBox chkSelect = (CheckBox)gvRow.FindControl("chkSelect");

            ViewState["EQUIVALENTRANK"] = ((RadLabel)gvRow.FindControl("lblRankId")).Text;

            //CheckBox chkSelect = ((CheckBox)e.Item.FindControl("chkSelect"));
            if (chkSelect.Checked == true)
                UpdateEquivalentRank();
            else
                DeleteEquivalentRank();


            gvRankList.Rebind();

        }
        catch (Exception ex)
        {
            BindData();
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
