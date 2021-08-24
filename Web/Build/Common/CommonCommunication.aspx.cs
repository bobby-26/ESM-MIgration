using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class Common_CommonCommunicationn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();
            toolbarNoonReporttap.AddButton("Send", "SEND", ToolBarDirection.Right);

            MenuSend.AccessRights = this.ViewState;
            MenuSend.MenuList = toolbarNoonReporttap.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = string.Empty;
                ViewState["TYPE"] = string.Empty;
                ViewState["REFERENCEID"] = string.Empty;

                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() != string.Empty) 
                {
                    ViewState["TYPE"] = Request.QueryString["Type"].ToString();
                }

                if (Request.QueryString["Referenceid"] != null && Request.QueryString["Referenceid"].ToString() != "")
                {
                    ViewState["REFERENCEID"] = Request.QueryString["Referenceid"].ToString();
                }
                if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"].ToString() != string.Empty)
                {
                    ViewState["VESSELID"] = Request.QueryString["Vesselid"].ToString();
                }
                else if (ViewState["VESSELID"].ToString() == string.Empty)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                }

            }

            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSend_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEND"))
            {
                if (!string.IsNullOrEmpty(ViewState["TYPE"].ToString()))
                {
                    Guid? g = Guid.Empty;
                    {
                        string message = txtcommunication.Text;
                        if (!IsValidMessage(message))
                        {
                            ucError.Visible = true ;
                            return;
                        }

                        PheonixCommonCommunication.CommonCommunicationInsert(General.GetNullableGuid(ViewState["REFERENCEID"].ToString()), message, int.Parse(ViewState["VESSELID"].ToString()),ViewState["TYPE"].ToString(), null, ref g);

                        PhoenixCommonFileAttachment.InsertAttachment(Request.Files, g.Value, PhoenixModule.REGISTERS, null, string.Empty,
                              string.Empty, ViewState["TYPE"].ToString(), int.Parse(ViewState["VESSELID"].ToString()));
                        BindData();
                        txtcommunication.Text = "";
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

    private bool IsValidMessage(string message)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(message))
            ucError.ErrorMessage = "Type some Comments.";

        return (!ucError.IsError);
    }

    protected void repCommunication_ItemDataBound(Object sender, RepeaterItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton attachment = (LinkButton)e.Item.FindControl("cmdAttachment");

            if (attachment != null)
            {
                attachment.Visible = SessionUtil.CanAccess(this.ViewState, attachment.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "1")
                {
                    HtmlImage html = new HtmlImage();
                    html.Src = Session["sitepath"] + "/css/" + Session["theme"].ToString() + "/images/" + "/attachment.png";
                    attachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=REGISTERS&type=" + ViewState["TYPE"] + "&u=n&VESSELID=" + drv["FLDVESSELID"] + "');return false;");

                }
                else

                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    attachment.Attributes.Add("style", "display:none");
                }
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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PheonixCommonCommunication.CommonCommunicationSearch(int.Parse(ViewState["VESSELID"].ToString()),
                                                                        General.GetNullableString(txtsearch.Text),                                                                      
                                                                      General.GetNullableDateTime(txtDateFrom.Text),
                                                                      General.GetNullableDateTime(txtDateTo.Text)
                                                                      ,ViewState["TYPE"].ToString(), null,
                                                                      sortexpression,
                                                                      sortdirection,
                                                                      int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                      General.ShowRecords(null),
                                                                      ref iRowCount,
                                                                      ref iTotalPageCount,
                                                                      General.GetNullableGuid(ViewState["REFERENCEID"].ToString()));

            repCommunication.DataSource = ds;
            repCommunication.DataBind();
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void search_onclick(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
    
}