using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationAttributeRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"];
                gvAttribute.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            ShowToolbar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvAttribute.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowToolbar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageLink(String.Format("javascript:openNewWindow('MoreInfo','','{0}/Inspection/InspectionRegulationRuleAttributeAdd.aspx'); return false;", Session["sitepath"]), "Add New Attribute", "", "ADDNEWATTRIBUTE", ToolBarDirection.Right);
        NewAttribute.AccessRights = this.ViewState;
        NewAttribute.MenuList = toolbar.Show();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionNewRegulation.AttributeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)ViewState["PAGENUMBER"]
                                                                            , gvAttribute.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);

        gvAttribute.DataSource = ds;
        gvAttribute.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvAttribute_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EDIT")
            {
                RadLabel AttributeId = (RadLabel)e.Item.FindControl("lblAttributeId");
                ViewState["AttributeId"] = AttributeId.Text;
                ViewState["IncludeYN"] = ((RadLabel)e.Item.FindControl("lblIncludeYN")).Text;
            }
            if (e.CommandName == "Add")
            {
                //RadTextBox AttributeCode = (RadTextBox)e.Item.FindControl("txtAttributeCodeAdd");
                //RadTextBox AttributeName = (RadTextBox)e.Item.FindControl("txtAttributeNameAdd");
                //RadRadioButtonList IncludeYN = (RadRadioButtonList)e.Item.FindControl("chkIncludeYNAdd");
                //bool includeYN = Convert.ToBoolean(IncludeYN.SelectedValue);
                //bool isDeletable = false;

                //if (IsValidInput(AttributeCode.Text, AttributeName.Text))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                //else
                //{
                //    PhoenixInspectionNewRegulation.AttributeInsert(AttributeCode.Text, AttributeName.Text, includeYN, isDeletable);
                //    ucStatus.Text = "Information Added";
                //}
            }

            if (e.CommandName == "UPDATE")
            {
                RadTextBox AttributeCode = (RadTextBox)e.Item.FindControl("txtAttributeCodeEdit");
                RadTextBox AttributeName = (RadTextBox)e.Item.FindControl("txtAttributeNameEdit");
                RadRadioButtonList IncludeYN = (RadRadioButtonList)e.Item.FindControl("chkIncludeYNEdit");
                bool includeYN = Convert.ToBoolean(IncludeYN.SelectedValue);


                if (IsValidInput(AttributeCode.Text, AttributeName.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionNewRegulation.AttributeUpdate(General.GetNullableGuid(ViewState["AttributeId"].ToString()), AttributeCode.Text, AttributeName.Text, includeYN);
                    ucStatus.Text = "Information Updated";
                }
            }
            if (e.CommandName == "DELETE")
            {
                RadLabel AttributeId = (RadLabel)e.Item.FindControl("lblAttributeId");

                ViewState["AttributeId"] = AttributeId.Text;

                PhoenixInspectionNewRegulation.AttributeDelete(General.GetNullableGuid(ViewState["AttributeId"].ToString()));
            }
            gvAttribute.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidInput(string AttributeCode, string AttributeName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (AttributeCode == "")
            ucError.ErrorMessage = "Code is required";

        if (AttributeName == "")
            ucError.ErrorMessage = "Attribute Name is required";

        return (ucError.IsError);
    }

    protected void gvAttribute_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttribute.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvAttribute_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
        {
            RadRadioButtonList includeYN = (RadRadioButtonList)e.Item.FindControl("chkIncludeYNEdit");
            if (ViewState["IncludeYN"].ToString() == "True")
            {
                includeYN.SelectedIndex = 0;
            }
            else
            {
                includeYN.SelectedIndex = 1;
            }
        }

        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
    }


}