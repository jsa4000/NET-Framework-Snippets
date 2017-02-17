﻿Imports System.Data
Imports Npgsql

Public Class Form1

    Private Sub Button1_Click(sender As System.Object, e As System.EventArgs) Handles Button1.Click

      
        Dim conn As NpgsqlConnection = New NpgsqlConnection("Server=192.168.0.4;Port=5432;" +
                                      "User Id=ANY;Password=ANY;Database=MPTDB;")
        conn.Open()

        Try
            ' Agregar una fila en una tabla
            ' INSERT INTO "STATION"("ACRONYM", "NAME") VALUES ('RC05', 'Hola');

            'Dim command1 As NpgsqlCommand = New NpgsqlCommand("insert into ""STATION"" values('RC078', 'Otro Receptor')", conn)
            'Dim rowsaffected As Int32 = command1.ExecuteNonQuery()
            'Console.WriteLine("Se agregaron {0} filas en tabla1", rowsaffected)

            ' REelazizar una transacción para la insercción de elementos en caso de que falle 

            ''Empezamos transacción
            'Dim transaction As IDbTransaction = conn.BeginTransaction()

            'Try
            '    Dim command1 As NpgsqlCommand = New NpgsqlCommand("insert into ""STATION"" values('RC078', 'Otro Receptor con Transacción')", conn)
            '    Dim rowsaffected As Int32 = command1.ExecuteNonQuery()
            '    Console.WriteLine("Se agregaron {0} filas en tabla1", rowsaffected)

            '    Dim command3 As NpgsqlCommand = New NpgsqlCommand("insert into ""STATION"" values('RC070', 'Otro Receptor con Transacción2')", conn)
            '    Dim rowsaffected3 As Int32 = command3.ExecuteNonQuery()
            '    Console.WriteLine("Se agregaron {0} filas en tabla1", rowsaffected3)

            '    'Commit transaction 
            '    transaction.Commit()

            'Catch ex As Exception
            '    transaction.Rollback()
            'Finally
            '    transaction.Dispose()
            'End Try

            'Hacemos un UPSERT con trannsacción

            ' Empezamos(transacción)
            Dim transaction As IDbTransaction = conn.BeginTransaction()

            Try
                Dim command1 As NpgsqlCommand = New NpgsqlCommand("WITH upsert AS (UPDATE ""STATION"" SET ""NAME""='Modificado TEST4!' WHERE ""ACRONYM""='TEST4' RETURNING *) " & _
                                                                  "INSERT INTO ""STATION"" (""ACRONYM"",""NAME"") SELECT 'TEST4', 'Nuevo43' WHERE NOT EXISTS (SELECT * FROM upsert)", conn)
                Dim rowsaffected As Int32 = command1.ExecuteNonQuery()
                Console.WriteLine("Se agregaron {0} filas en tabla1", rowsaffected)

                Dim command3 As NpgsqlCommand = New NpgsqlCommand("WITH upsert AS (UPDATE ""STATION"" SET ""NAME""='Modificado TEST4!' WHERE ""ACRONYM""='TEST4' RETURNING *) " & _
                                                                 "INSERT INTO ""STATION"" (""ACRONYM"",""NAME"") SELECT 'TEST4', 'Nuevo43' WHERE NOT EXISTS (SELECT * FROM upsert)", conn)
                Dim rowsaffected3 As Int32 = command3.ExecuteNonQuery()
                Console.WriteLine("Se agregaron {0} filas en tabla1", rowsaffected3)

                'Commit transaction 
                transaction.Commit()

            Catch ex As Exception
                transaction.Rollback()
            Finally
                transaction.Dispose()
            End Try

            ' Obteniendo un conjunto de filas de una consulta
            Dim command2 As NpgsqlCommand = New NpgsqlCommand("select * from ""STATION""", conn)
            Dim dr As NpgsqlDataReader = command2.ExecuteReader()

            'Do While (dr.Read())
            '    For i As Integer = 0 To dr.FieldCount - 1
            '        Console.Write("{0} \t", dr(i))
            '    Next
            '    Console.WriteLine()
            'Loop

            Do While (dr.Read())
                'To acces to the column directly
                Console.WriteLine("{0} \t", dr("ACRONYM") & " " & dr("NAME"))

            Loop

            WriteLine("End Process")

        Catch ex As Exception

        Finally
            'Close the  connection
            conn.Close()
        End Try

    End Sub
End Class
