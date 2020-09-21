using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WeX.Modules.fun
{
    public class MineSweeper : ModuleBase<SocketCommandContext>
    {
        [Command("minesweeper")]
        public async Task MS()
        {
            //TEN KOD JEST PONAD ROK STARSZY NIŻ TEN, KTÓRY PISZĘ TERAZ 21.07.2020 ~ uzer
            //-----------------------------------------------------ZMIENNE----------------------------------------------------
            int[,] maingameINT = new int[10, 10];
            int converter = 0, liczbaBomb = 0;
            System.Random rand = new Random(System.DateTime.Now.Millisecond);
            string final = string.Empty;
            //-----------------------------------------------------PLANSZA----------------------------------------------------
            //losowanie liczb do tabeli maingameINT
            for (int y = 0; y < maingameINT.Length / 10; y++)
            {
                for (int x = 0; x < maingameINT.Length / 10; x++)
                {
                    converter = rand.Next(1, 100);
                    if (x == 0 || y == 0 || x == 9 || y == 9)
                    {
                        maingameINT[x, y] = 10;
                    }
                    else if (converter > 20)
                    {
                        maingameINT[x, y] = 1;
                    }
                    else
                    {
                        maingameINT[x, y] = 0;
                    }
                }
            }

            //Sprawdzanie bomb do okoła
            for (int yy = 0; yy < maingameINT.Length / 10; yy++)
            {
                for (int xx = 0; xx < maingameINT.Length / 10; xx++)
                {
                    if (xx == 0 || yy == 0 || xx == 9 || yy == 9)
                    {

                    }
                    else if (maingameINT[xx, yy] == 1)
                    {
                        liczbaBomb = 0;
                        // lvl 1
                        if (maingameINT[xx - 1, yy - 1] == 0)
                        {
                            liczbaBomb++;
                        }
                        if (maingameINT[xx - 1, yy] == 0)
                        {
                            liczbaBomb++;
                        }
                        if (maingameINT[xx - 1, yy + 1] == 0)
                        {
                            liczbaBomb++;
                        }
                        // lvl 2
                        if (maingameINT[xx, yy - 1] == 0)
                        {
                            liczbaBomb++;
                        }
                        if (maingameINT[xx, yy + 1] == 0)
                        {
                            liczbaBomb++;
                        }
                        // lvl 3
                        if (maingameINT[xx + 1, yy - 1] == 0)
                        {
                            liczbaBomb++;
                        }
                        if (maingameINT[xx + 1, yy] == 0)
                        {
                            liczbaBomb++;
                        }
                        if (maingameINT[xx + 1, yy + 1] == 0)
                        {
                            liczbaBomb++;
                        }
                        //zmiana ilosci na bloku
                        if (liczbaBomb == 0)
                        {
                            maingameINT[xx, yy] = 11;
                        }
                        else
                        {
                            maingameINT[xx, yy] = liczbaBomb;
                        }
                    }
                }
            }

            //Renderowanie obrazu w konsoli
            for (int yyy = 0; yyy < maingameINT.Length / 10; yyy++)
            {
                for (int xxx = 0; xxx < maingameINT.Length / 10; xxx++)
                {
                    if (xxx == 0 || yyy == 0 || xxx == 9 || yyy == 9)
                    {
                    }
                    else if (maingameINT[xxx, yyy] == 0)
                    {
                        final += "|| :bomb: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 11)
                    {
                        final += "|| :black_medium_square: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 1)
                    {
                        final += "|| :one: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 2)
                    {
                        final += "|| :two: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 3)
                    {
                        final += "|| :three: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 4)
                    {
                        final += "|| :four: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 5)
                    {
                        final += "|| :five: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 6)
                    {
                        final += "|| :six: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 7)
                    {
                        final += "|| :seven: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 8)
                    {
                        final += "|| :eight: ||";
                    }
                    else if (maingameINT[xxx, yyy] == 9)
                    {
                        final += "|| :nine: ||";
                    }

                }
                final += "\n";
            }
            await ReplyAsync(final);

        }
    }
}
