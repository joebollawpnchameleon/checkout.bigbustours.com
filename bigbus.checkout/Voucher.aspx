﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Voucher.aspx.cs" Inherits="bigbus.checkout.Voucher" %>

<html>
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
<meta name="robots" content="noindex">

<title>Big Bus Tours Voucher Page</title>
<style type="text/css">
body{
    font-family: Arial,Helvetica,sans-serif;
    font-size: 50px;
    margin: 0;}

    p{margin: 0;}

    .page-breaker {
        height:1px;
        display: block;
        page-break-after: always;
    }

    .wrapper{
        border: 0.12em solid #000;
        box-sizing: border-box;
        background-size: cover;
        width: 21.6em;
        height: 9.6em;
        background: url('/Content/Images/design/watermark.jpg') no-repeat;
        background-origin: border-box;
        margin-bottom: 20px;
        position: relative;
    }

        .lhs{/* left hand side */
            margin: 0.380em 0 0 0.64em;
            width: 15.880em;
            /*height: 6.65em;*/
            float: left;
        }

            .branding{/* logo */
                height: 2.040em;
                margin-left: -0.14em;
            }

                .branding img{
                    height: 2.040em;
                    display: block;
                    float: left;
                    margin-right: 15px;
                }

            .specs{
                height: 3.060em;
                /*line-height: 0.75em;*/
                margin-top: 0.40em;
            }

                .specs > div{
                    float: left;
                    width: 100%;
                    margin-bottom: 5px;
                }

                .specs p{
                    width: 19%;
                    /*height: 3.1em;*/
                    font-size: 0.28em;
                    line-height: 1.786em;
                    float:left;
                }

                .specs p + p{
                    width: 80%;
                    font-size: 18px;
                    line-height: 1.190em;
                    font-weight: bold;
                }

            .small-print{/* smallprint */
                left: 32px;
                font-size: 0.30em;
                position: absolute;
                bottom: 5px;
                width: 52.933em;
            }

                .small-print p:first-child{
                    font-weight: bold;
                    margin-bottom: 0.020em;
                }

        .rhs{/* right hand side */
            width: 4.300em;
            height: 6.920em;
            margin: 0.200em 0.440em 0 0 ;
            float: right;
        }

            .summary{
                height: 3.800em;
                /*margin-top: 30px;*/
            }

                .summary div{
                    float: left;
                    clear: left;
                    width: 100%;
                }

                .summary p{
                    float: right;
                    line-height: 43px;
                    font-size: 28px;
                    font-weight: bold;
                    text-align: center;
                    width: 1.75em;
                    /*width: 2.870em;*/
                    line-height: 55px;
                }

                .summary p:first-child{
                    float: left;
                    /*width: 4.286em;*/
                    font-size: 28px;
                    text-align: left;
                    width: auto;
                    font-weight: normal;
                }

            .b-code{
                position: absolute;
                bottom: 0.2em;
                right: 0.440em;
                width: 4.000em;
            }

                .b-code img{
                    width: 200px;
                    /*height: 200px;*/
                }

                .b-code div{
                     font-size:0.5em;
                     text-align: center;
                }
</style>
</head>
<body>

<asp:PlaceHolder runat="server" id="plcAllVouchersContent"/>

</body>
</html>
