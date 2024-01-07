import React, { useEffect, useState } from "react";
import './styles/recipientBar.css';
import axios from 'axios';
import more from "../../assets/images/more-points.svg";
import { DropdownMenuItemProps } from "./Interfaces/Conversation"

const ConversationActions: React.FC<DropdownMenuItemProps> = ({
    conversationGuid,
    isMuted,
    isPinned,
    isBlocked }) => {

    const [userId, setUserId] = useState<string>("");

    const [isOpen, setIsOpen] = useState(false);

    const handleOpen = () => {
        setIsOpen(!isOpen);
    };

    const storedUserId = sessionStorage.getItem('userId');
    useEffect(() => {
        if (storedUserId) {
            console.log(storedUserId)
            setUserId(storedUserId);
        }
    })


    const baseUrl = 'https://localhost:7014/api';
    const pinMessage = () => {
        const body = {
            isPinned: !isPinned,
            conversationGuid: conversationGuid,
            userId: userId
        }
        axios.post(baseUrl + `/Conversations/Pin`, body)
            .then(response => {
                console.log(response);
            })
            .catch(error => {
                console.log(error);
            });
    };

    const offNotification = () => {
        const body = {
            isMuted: !isMuted,
            conversationGuid: conversationGuid,
            userId: userId
        }
        axios.post(baseUrl + `/Conversations/Mute`, body)
            .then(response => {
                console.log(response);
            })
            .catch(error => {
                console.log(error);
            });
    }

    const blockConversation = () => {
        const body = {
            isMuted: !isBlocked,
            conversationGuid: conversationGuid,
            userId: userId
        }
        axios.post(baseUrl + `/Conversations/Block`, body)
            .then(response => {
                console.log(response);
            })
            .catch(error => {
                console.log(error);
            });
    }

    const reportConversation = () => {

    }

    return (
        <div className="chat-dropdown-button">
            <div className="dropdown-toggle-button" onClick={handleOpen}>
                <img className="dropdown-image" src={more} />
            </div>
            <div className={`dropdown-menu ${isOpen ? 'open' : ''}`}>
                <button className="dropdown-pin" onClick={pinMessage}>
                Закріпити повідомлення
                </button>
                <button className="dropdown-off-notification" onClick={offNotification}>
                Вимкніть сповіщення
                </button>
                <button className="dropdown-block" onClick={blockConversation}>
                Блокувати розмову
                </button>
                <button className="dropdown-report" onClick={reportConversation}>
                Повідомити про розмову
                </button>
            </div>
        </div>
    );
};

export default ConversationActions;