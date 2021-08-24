using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class RegistersContractCBAExpression : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "addcomponent")
            {

                InsertCBAExpression(General.GetNullableGuid(Request.QueryString["cid"]).Value, General.GetNullableGuid(lstComponent.SelectedValue), null);
            }

            if (Request["__EVENTARGUMENT"] != null && Request["__EVENTARGUMENT"] == "addoperator")
            {

                InsertCBAExpression(General.GetNullableGuid(Request.QueryString["cid"]).Value, null, lstOperator.SelectedValue);
            }
            if (!IsPostBack)
            {
                EditCBA();
                BindComponentList();
                lstComponent.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(lstComponent, "addcomponent"));
                lstOperator.Attributes.Add("ondblclick", ClientScript.GetPostBackEventReference(lstComponent, "addoperator"));
            }
            DisplayFormula();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvFormula.SelectedIndexes.Clear();
        gvFormula.EditIndexes.Clear();
        gvFormula.DataSource = null;
        gvFormula.Rebind();
    }
    protected void BindData()
    {
        DataTable dt = PhoenixRegistersContractCBAExpression.ListCBAComponentExpressionFormula(General.GetNullableGuid(Request.QueryString["cid"]));
        gvFormula.DataSource = dt;
    }

    protected void gvFormula_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                if (drv.DataView.Count == e.Item.RowIndex + 1)
                    db.Visible = true;
                else
                    db.Visible = false;
            }
        }


    }
    protected void gvFormula_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string dtkey = ((RadLabel)e.Item.FindControl("lblDtKey")).Text;
                PhoenixRegistersContractCBAExpression.DeleteCBAComponentExpression(General.GetNullableGuid(Request.QueryString["cid"]).Value, new Guid(dtkey));
                Rebind(); DisplayFormula();
            }
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvFormula_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindComponentList()
    {
        lstComponent.Items.Clear();
        DataTable dt = PhoenixRegistersContractCBAExpression.ListCBAComponentExpression(General.GetNullableGuid(Request.QueryString["cid"]));
        lstComponent.DataSource = dt;
        lstComponent.DataTextField = "FLDCOMPONENTNAME";
        lstComponent.DataValueField = "FLDCOMPONENTID";
        lstComponent.DataBind();
    }
    private void DisplayFormula()
    {
        DataTable dt = PhoenixRegistersContractCBAExpression.FetchCBAComponentExpressionFormula(General.GetNullableGuid(Request.QueryString["cid"]));
        if (dt.Rows.Count > 0)
        {
            txtExpression.Text = dt.Rows[0][0].ToString();
        }
    }
    private void InsertCBAExpression(Guid ComponentId, Guid? ExprComponentId, string Operator)
    {
        PhoenixRegistersContractCBAExpression.InsertCBAComponentExpression(ComponentId, ExprComponentId, Operator);
    }
    private void EditCBA()
    {
        try
        {
            DataTable dt = PhoenixRegistersContract.EditCBAContract(General.GetNullableGuid(Request.QueryString["cid"]).Value);
            if (dt.Rows.Count > 0)
            {
                txtComponent.Text = dt.Rows[0]["FLDCOMPONENTNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
