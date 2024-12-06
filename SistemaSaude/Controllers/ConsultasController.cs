using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using SistemaSaude.Models;

public class ConsultasController : Controller
{
    private readonly string connection = "Server=DESKTOP-2I0FSTB\\GABSQL;Database=SistemaSaude; trusted_connection=true; trustservercertificate=true";

    public ActionResult Index()
    {
        List<Consulta> consultas = new List<Consulta>();
        List<Paciente> pacientes = new List<Paciente>();

        using (SqlConnection conn = new SqlConnection(connection))
        {
            string query = @"
                SELECT c.*, p.NomePaciente 
                FROM Consultas c 
                INNER JOIN Pacientes p ON c.IdPaciente = p.IdPaciente";
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();

            SqlDataReader consultaReader = cmd.ExecuteReader();
            while (consultaReader.Read())
            {
                consultas.Add(new Consulta
                {
                    IdConsulta = (int)consultaReader["IdConsulta"],
                    IdPaciente = (int)consultaReader["IdPaciente"],
                    DataConsulta = (DateTime)consultaReader["DataConsulta"],
                    Observacoes = consultaReader["Observacoes"]?.ToString(),
                    NomePaciente = consultaReader["NomePaciente"].ToString()
                });
            }
            consultaReader.Close();

            string pQuery = "SELECT * FROM Pacientes";
            SqlCommand pCmd = new SqlCommand(pQuery, conn);
            SqlDataReader pacienteReader = pCmd.ExecuteReader();
            while (pacienteReader.Read())
            {
                pacientes.Add(new Paciente
                {
                    IdPaciente = (int)pacienteReader["IdPaciente"],
                    NomePaciente = pacienteReader["NomePaciente"].ToString()
                });
            }
        }
        ViewBag.Pacientes = pacientes;
        return View(consultas);
    }

    [HttpPost]
    public ActionResult CreateConsulta(Consulta consulta)
    {
        using (SqlConnection conn = new SqlConnection(connection))
        {
            string query = "INSERT INTO Consultas (IdPaciente, DataConsulta, Observacoes) VALUES (@IdPaciente, @DataConsulta, @Observacoes)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IdPaciente", consulta.IdPaciente);
            cmd.Parameters.AddWithValue("@DataConsulta", consulta.DataConsulta);
            cmd.Parameters.AddWithValue("@Observacoes", consulta.Observacoes);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult DeleteConsulta(int id)
    {
        using (SqlConnection conn = new SqlConnection(connection))
        {
            string query = "DELETE FROM Consultas WHERE IdConsulta = @IdConsulta";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IdConsulta", id);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("Index");
    }

    public ActionResult UpdateConsulta(int id)
    {
            Consulta consulta = null;
            List<Paciente> pacientes = new List<Paciente>();

            using (SqlConnection conn = new SqlConnection(connection))
            {
                string query = "SELECT * FROM Consultas WHERE IdConsulta = @IdConsulta";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdConsulta", id);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    consulta = new Consulta
                    {
                        IdConsulta = (int)reader["IdConsulta"],
                        IdPaciente = (int)reader["IdPaciente"],
                        DataConsulta = (DateTime)reader["DataConsulta"],
                        Observacoes = reader["Observacoes"].ToString()
                    };
                }
                reader.Close();
            }
            return View(consulta);
        }

    [HttpPost]
    public ActionResult SaveUpdateConsulta(Consulta consulta)
    {
        using (SqlConnection conn = new SqlConnection(connection))
        {
            string query = "UPDATE Consultas SET DataConsulta = @DataConsulta, Observacoes = @Observacoes WHERE IdConsulta = @IdConsulta";
           
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@IdConsulta", consulta.IdConsulta);
            cmd.Parameters.AddWithValue("@IdPaciente", consulta.IdPaciente);
            cmd.Parameters.AddWithValue("@DataConsulta", consulta.DataConsulta);
            cmd.Parameters.AddWithValue("@Observacoes", consulta.Observacoes);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        return RedirectToAction("Index");
    }
}





