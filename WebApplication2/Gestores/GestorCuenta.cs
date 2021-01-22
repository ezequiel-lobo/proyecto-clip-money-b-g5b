﻿using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebApplication2.Models;


namespace WebApplication2.Gestores
{
    public class GestorCuenta
    {
        public int ObtenerIdCuenta(int idUsuario)
        {

            string strConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            int idCuenta;

            using (SqlConnection connec = new SqlConnection(strConn))
            {
                connec.Open();

                SqlCommand comm = new SqlCommand("obtener_id_cuenta", connec);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@idUsuario", idUsuario));


                idCuenta = Convert.ToInt32(comm.ExecuteScalar());
                
            }
            return idCuenta;
        }

        public decimal ModificarSaldo(int idCuenta, Operaciones tipoOperacion)
        {
            string strConn = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            decimal saldo = 0;
            decimal nuevoSaldo = 0;

            using (SqlConnection connec = new SqlConnection(strConn))
            {
                connec.Open();

                SqlCommand comm = new SqlCommand("obtener_saldo", connec);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@idCuenta", idCuenta));
                                
                SqlDataReader reader = comm.ExecuteReader();

                if (reader.HasRows)
                {
                    saldo = reader.GetInt32(2);
                }

            }

            if (tipoOperacion.Nombre == "Deposito")
            {
                nuevoSaldo = saldo + tipoOperacion.Monto;
            }
            if (tipoOperacion.Nombre == "Extraccion")
            {
                nuevoSaldo = saldo - tipoOperacion.Monto;
            }
            if (tipoOperacion.Nombre == "Transferencia")
            {
                nuevoSaldo = saldo - tipoOperacion.Monto;
            }
            if (tipoOperacion.Nombre == "Giro al Descubierto")
            {
                if (tipoOperacion.Monto == (saldo * 0.10M))
                {
                    nuevoSaldo = saldo - tipoOperacion.Monto;
                }
                else
                {
                    //Mensaje de error "Monto no apto para giro al descubierto"
                    //MessageBox.Show($"");
                    //Request.Flash("success", "Monto no apto para giro al descubierto");
                }

            }
            using (SqlConnection connec = new SqlConnection(strConn))
            {
                connec.Open();

                SqlCommand comm = new SqlCommand("modificar_saldo", connec);
                comm.CommandType = System.Data.CommandType.StoredProcedure;
                comm.Parameters.Add(new SqlParameter("@idCuenta", idCuenta));
                comm.Parameters.Add(new SqlParameter("@saldo", nuevoSaldo));
                comm.ExecuteNonQuery();

            }
            return nuevoSaldo;
        }
    }
}