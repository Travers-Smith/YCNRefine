import React, { useEffect, useState } from "react";
import GlobalContext from "./context/GlobalContext";
import Login from "./scenes/Login/Login";
import { Navigate, Route, Routes, useNavigate } from "react-router-dom";
import AppRoutes from "./AppRoutes";
import Layout from './layouts/Layout';
import useSafeDataFetch from "./hooks/useSafeDataFetch";

const YCNRefine = () => {
    const safeDataFetch = useSafeDataFetch()[1];
    const navigate = useNavigate();
    const [checkedLoggedIn, setCheckedLoggedIn] = useState(false);
    const [user, setUser] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            const response = await safeDataFetch({
                url: "/identity/get-user"
            });

            if(!response.data){
                navigate("/login")
            } else {
                setUser(response.data)
            }

            setCheckedLoggedIn(true);
        };

        fetchData();
    }, []);

    return (
        <GlobalContext.Provider
            value={{
                user
            }}
        >
            <Routes>
                <Route path="/login" element={<Login/>}/>
                <Route path="/" element={<Layout/>}>
                    {
                        checkedLoggedIn &&
                            <Route path="/" element={<Navigate to="/chat-labeller" replace/>}/>
                    }
                    {
                        AppRoutes.map((route, index) => {
                            const { element, ...rest } = route;
                            return <Route key={index} {...rest} element={element} />;
                        })
                    }
                </Route>
            </Routes>
        </GlobalContext.Provider>
    )
};

export default YCNRefine;