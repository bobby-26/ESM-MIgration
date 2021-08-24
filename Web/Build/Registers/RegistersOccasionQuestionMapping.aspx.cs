using System;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersOccasionQuestionMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Done", "DONE",ToolBarDirection.Right);       
        MenuRemarks.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            ViewState["PROFILECATEGORYID"] = "";
            ViewState["PROFILEQUESTIONID"] = "";
            ViewState["RANKCATEGORYID"] = "";
            ViewState["PROFILECATEGORYID"] = Request.QueryString["categoryid"].ToString();
            ViewState["PROFILEQUESTIONID"] = Request.QueryString["questionid"].ToString();
            ViewState["RANKCATEGORYID"] = Request.QueryString["rankcategoryid"].ToString();
        }
    }

    private void BindSelectedRemarks()
    {
        try
        {
            DataSet ds = PhoenixRegistersAppraisalProfileQuestion.ListSelectedoccsionQuestion( General.GetNullableInteger(ViewState["PROFILEQUESTIONID"].ToString())
                                                                                                ,General.GetNullableInteger(ViewState["PROFILECATEGORYID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSelectedRemarks.DataSource = ds;                
            }
            else
            {
                gvSelectedRemarks.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            DataTable dt = PhoenixRegistersAppraisalProfileQuestion.ListOccasionunlist( General.GetNullableInteger(ViewState["PROFILECATEGORYID"].ToString())
                                                                                                    , General.GetNullableInteger(ViewState["PROFILEQUESTIONID"].ToString())
                                                                                                    , General.GetNullableInteger(ViewState["RANKCATEGORYID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                gvRemarks.DataSource = dt;             
            }
            else
            {
                gvRemarks.DataSource = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("DONE"))
            {
                if (gvRemarks.MasterTableView.Items.Count > 0)
                {
                    StringBuilder strRemarksId = new StringBuilder();


                    foreach (GridDataItem gr in gvRemarks.MasterTableView.Items)
                    {
                        RadCheckBox chkSelect = (RadCheckBox)gr.FindControl("chkSelect");
                        RadLabel lblOccasionId = (RadLabel)gr.FindControl("lblOccasionId");

                        if (chkSelect.Checked==true)
                        {
                            PhoenixRegistersAppraisalProfileQuestion.InsertoccasionquestionMapping(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                    , int.Parse(ViewState["PROFILECATEGORYID"].ToString())                                                                                                    
                                                                                                    , int.Parse(ViewState["PROFILEQUESTIONID"].ToString())
                                                                                                    , int.Parse(lblOccasionId.Text));
                        }
                    }
                }
            }
            BindSelectedRemarks();
            BindData();
            gvSelectedRemarks.Rebind();
            gvRemarks.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSelectedRemarks_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "DELETE")
        {
            string MappingId = ((RadLabel)e.Item.FindControl("lblMappingId")).Text;
            PhoenixRegistersAppraisalProfileQuestion.DeleteOccasionQuestionMapping(new Guid(MappingId));
            //ViewState["REMARKSIDLIST"] = null;
            BindSelectedRemarks();
            BindData();
            gvSelectedRemarks.Rebind();
            gvRemarks.Rebind();
        }
    }

    protected void gvSelectedRemarks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindSelectedRemarks();
    }

    protected void gvSelectedRemarks_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }            
        }
    }
    protected void gvRemarks_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
