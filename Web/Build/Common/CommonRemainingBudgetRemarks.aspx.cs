using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using System.Web;
using System.Web.UI;

public partial class CommonRemainingBudgetRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE");
            MenuFormDetail.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if ((Request.QueryString["vesselaccountid"] != null) && (Request.QueryString["vesselaccountid"] != ""))
                {
                    ViewState["VESSELACCOUNTID"] = Request.QueryString["vesselaccountid"].ToString();
                }
                if ((Request.QueryString["month"] != null) && (Request.QueryString["month"] != ""))
                {
                    ViewState["MONTH"] = Request.QueryString["month"].ToString();
                }
                if ((Request.QueryString["year"] != null) && (Request.QueryString["year"] != ""))
                {
                    ViewState["YEAR"] = Request.QueryString["year"].ToString();
                }
                if ((Request.QueryString["budgetgroupid"] != null) && (Request.QueryString["budgetgroupid"] != ""))
                {
                    ViewState["BUDGETGROUPID"] = Request.QueryString["budgetgroupid"].ToString();
                }
                ViewState["REMARKSID"] = "";
                BindData();
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

        DataSet ds = new DataSet();
        ds = PhoenixCommonBudgetOpeningBalance.RemainingBudgetRemarksEdit(int.Parse(ViewState["VESSELACCOUNTID"].ToString())
                                                                    ,int.Parse(ViewState["YEAR"].ToString())
                                                                    ,int.Parse(ViewState["MONTH"].ToString())
                                                                    ,int.Parse(ViewState["BUDGETGROUPID"].ToString())
                                                            );
        if (ds.Tables[0].Rows.Count > 0)
        {           
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormDetails.Content = dr["FLDREMARKS"].ToString();
            ViewState["REMARKSID"] = dr["FLDREMAININGBUDGETREMARKSID"].ToString();
        }        
    }

    protected void MenuFormDetail_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if ((Request.QueryString["VESSELACCOUNTID"] != null) && (Request.QueryString["VESSELACCOUNTID"] != ""))
                {
                    if (General.GetNullableGuid(ViewState["REMARKSID"].ToString()) == null)
                    {
                        PhoenixCommonBudgetOpeningBalance.RemainingBudgetRemarksInsert(int.Parse(ViewState["VESSELACCOUNTID"].ToString())
                                                                    , int.Parse(ViewState["YEAR"].ToString())
                                                                    , int.Parse(ViewState["MONTH"].ToString())
                                                                    , int.Parse(ViewState["BUDGETGROUPID"].ToString())
                                                                    , txtFormDetails.Content
                                                        );
                    }
                    else
                    {
                        PhoenixCommonBudgetOpeningBalance.RemainingBudgetRemarksUpdate(new Guid(ViewState["REMARKSID"].ToString())
                                                                    , txtFormDetails.Content
                                                                    );
                    }
                    ucStatus.Text = "Data has been Saved";
                    ucStatus.Visible = true; 
                }
              
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
 
    }

    protected void btnInsertPic_Click(object sender, EventArgs e)
    {
        try
        {            
            if (Request.Files.Count > 0 && txtFormDetails.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
            {
                Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PURCHASE, null, ".jpg,.png,.gif");
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
                DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
                if (dr.Length > 0)
                   txtFormDetails.Content = txtFormDetails.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() +"\" />";
            }
            else
            {
                ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
                ucError.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }       
    }
}
